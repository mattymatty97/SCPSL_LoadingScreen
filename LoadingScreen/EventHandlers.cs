
using System;
using System.Linq;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace LoadingScreen
{
    public class EventHandlers : IEventHandlerPlayerJoin, IEventHandlerWaitingForPlayers, IEventHandlerRoundStart
    {
        private readonly LoadingScreen plugin;

        private bool preRound = false;

        public EventHandlers(LoadingScreen plugin)
        {
            this.plugin = plugin;
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (preRound)
            {
                if (ev.Player.TeamRole.Team == Smod2.API.Team.NONE)
                {
                    Smod2.API.Door door = getRndDoor();
                    ev.Player.Teleport(new Vector(door.Position.x + 1f, door.Position.y + 1f, door.Position.z + 1f),
                        false);
                }
            }
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            plugin.RefreshConfig();
            preRound = true;
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            preRound = false;
        }

        private Smod2.API.Door getRndDoor()
        {
            Random rnd = new Random();
            Smod2.API.Door res;
            if (plugin.doors[0] == "*")
            {
                res = plugin.Server.Map.GetDoors().OrderBy(x=>rnd.Next(100)).FirstOrDefault();
            }
            else
            {
                res = plugin.Server.Map.GetDoors().OrderBy(x=>rnd.Next(100)).FirstOrDefault(x => plugin.doors.Contains(x.Name));
            }

            return res;
        }
    }
}
