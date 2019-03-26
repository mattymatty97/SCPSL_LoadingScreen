using System;
using System.Collections.Generic;
using System.Linq;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace LoadingScreen
{
    public class EventHandlers : IEventHandlerPlayerJoin, IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerFixedUpdate
    {
        private readonly LoadingScreen plugin;

        private Door[] doors = { };

        private readonly Random rnd = new Random();

        private DateTime? nextRefreshTime;
        private bool preRound;

        public EventHandlers(LoadingScreen plugin)
        {
            this.plugin = plugin;
        }

        public Door GetRndDoor()
        {
            return doors.Any() ? doors[rnd.Next(0, doors.Length)] : null;
        }

        public static void TeleportToDoor(Player player, Door door)
        {
            if (door != null && player != null && player.TeamRole.Team == Team.NONE)
            {
                player.Teleport(door.Position + Vector.Up);
            }
        }

        public void TeleportToRandomDoors(params Player[] players)
        {
            foreach (Player player in players)
            {
                TeleportToDoor(player, GetRndDoor());
            }
        }

        public void TeleportToRandomDoors(IEnumerable<Player> players)
        {
            foreach (Player player in players)
            {
                TeleportToDoor(player, GetRndDoor());
            }
        }

        public void OnWaitingForPlayers(WaitingForPlayersEvent ev)
        {
            plugin.RefreshConfig();

            doors = plugin.doors.Contains("*") ? ev.Server.Map.GetDoors().ToArray() : ev.Server.Map.GetDoors().Where(door => plugin.doors.Contains(door.Name.ToUpper())).ToArray();

            nextRefreshTime = null;

            plugin.Info($"Available Doors: {doors.Length}");
            preRound = true;
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            preRound = false;
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (!preRound)
                return;

            if (nextRefreshTime == null)
                nextRefreshTime = DateTime.Now.AddSeconds(plugin.seconds);

            TeleportToRandomDoors(ev.Player);
        }

        public void OnFixedUpdate(FixedUpdateEvent ev)
        {
            if (!preRound || nextRefreshTime == null || nextRefreshTime > DateTime.Now)
                return;

            List<Player> players = plugin.Server.GetPlayers();

            if (players.Any())
            {
                TeleportToRandomDoors(players);

                nextRefreshTime = DateTime.Now.AddSeconds(plugin.seconds);
            }
            else
            {
                nextRefreshTime = null;
            }
        }
    }
}
