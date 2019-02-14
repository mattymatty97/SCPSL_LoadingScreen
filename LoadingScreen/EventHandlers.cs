
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

        private Smod2.API.Door[] doors;
        private Random rnd = new Random();

        private bool preRound = false;

        public EventHandlers(LoadingScreen plugin)
        {
            this.plugin = plugin;
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            plugin.RefreshConfig();
            if (plugin.doors.Contains("*"))
                doors = ev.Server.Map.GetDoors().ToArray();
            else
                doors = ev.Server.Map.GetDoors().Where(x => plugin.doors.Contains(x.Name.ToUpper())).ToArray();
            preRound = true;
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (preRound)
            {
                if (ev.Player.TeamRole.Team == Smod2.API.Team.NONE)
                {
                    Smod2.API.Door door = getRndDoor();
                    ev.Player.Teleport(new Vector(door.Position.x + 1f, door.Position.y + 1f, door.Position.z + 1f));
                }

            }
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            preRound = false;
        }

        private Smod2.API.Door getRndDoor()
        {
            return doors[rnd.Next(0, doors.Length)];
        }
    }
}
