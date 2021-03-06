using System;
using EXILED;
using EXILED.Extensions;
using EXILED.Patches;
using MEC;

namespace ReliveThePast
{
    public class EventHandlers
    {
        Random randNum = new Random();
        float RespawnTimerValue = (float) Plugin.ReliveRespawnTimer;
        bool IsWarheadDetonated;
        bool IsDecontanimationActivated;
        bool AllowRespawning = false;

        public void RunOnPlayerDeath(ref PlayerDeathEvent d)
        {
            ReferenceHub hub = d.Player;
            IsWarheadDetonated = Map.IsNukeDetonated;
            IsDecontanimationActivated = Map.IsLCZDecontaminated;
            if (AllowRespawning == true)
                Timing.CallDelayed(RespawnTimerValue, () => RevivePlayer(hub));
        }

        public void RunOnRoundRestart()
        {
            AllowRespawning = false;
        }

        public void RunOnCommand(ref RACommandEvent r)
        {
            try
            {
                string arg = r.Command;
                ReferenceHub sender = r.Sender.SenderId == "SERVER CONSOLE" || r.Sender.SenderId == "GAME CONSOLE" ? PlayerManager.localPlayer.GetPlayer() : Player.GetPlayer(r.Sender.SenderId);

                switch (arg.ToLower())
                {
                    case "allowautorespawn":
                        r.Allow = false;
                        if (!sender.CheckPermission("rtp.allow"))
                        {
                            r.Sender.RAMessage("You are not authorized to use this command");
                            return;
                        }
                        if (AllowRespawning == false)
                        {
                            r.Sender.RAMessage("Auto respawning enabled!");
                            Map.Broadcast("<color=green>Auto respawning enabled!</color>", 5);
                            AllowRespawning = true;
                        }
                        else
                        { 
                            r.Sender.RAMessage("Auto respawning disabled!");
                            Map.Broadcast("<color=red>Auto respawning disabled!</color>", 5);
                            AllowRespawning = false;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                Log.Info("There was an error handling this command");
            }
        }

        public void RevivePlayer(ReferenceHub rh)
        {
            if ( rh.GetRole() != RoleType.Spectator ) return;
            int num = randNum.Next(0, 7);

            switch (num)
            {
                case 0:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfCadet, rh.gameObject);
                    break;
                case 1:  
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                    {
                        rh.characterClassManager.SetPlayersClass(RoleType.ClassD, rh.gameObject);
                        return;
                    }
                    if (!IsWarheadDetonated && IsDecontanimationActivated)
                    {
                        rh.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, rh.gameObject);
                        return;
                    }
                    if (IsWarheadDetonated || IsDecontanimationActivated)
                        rh.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, rh.gameObject);
                    break;
                case 2:
                    if (!IsWarheadDetonated)
                    {
                        rh.characterClassManager.SetPlayersClass(RoleType.FacilityGuard, rh.gameObject);
                        return;
                    }
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfCommander, rh.gameObject);
                    break;
                case 3:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, rh.gameObject);
                    break;
                case 4:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfScientist, rh.gameObject);
                    break;
                case 5:
                    rh.characterClassManager.SetPlayersClass(RoleType.ChaosInsurgency, rh.gameObject);
                    break;
                case 6:
                    if (!IsWarheadDetonated && !IsDecontanimationActivated)
                    {
                        rh.characterClassManager.SetPlayersClass(RoleType.Scientist, rh.gameObject);
                        return;
                    }
                    if (!IsWarheadDetonated && IsDecontanimationActivated)
                    {
                        rh.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, rh.gameObject);
                        return;
                    }
                    if (IsWarheadDetonated || IsDecontanimationActivated)
                        rh.characterClassManager.SetPlayersClass(RoleType.NtfLieutenant, rh.gameObject);
                    break;
                case 7:
                    rh.characterClassManager.SetPlayersClass(RoleType.NtfCommander, rh.gameObject);
                    break;
            }
        }
    }
}

