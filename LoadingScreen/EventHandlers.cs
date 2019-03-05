using System;
using System.Linq;
using ServerMod2.API;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;
using UnityEngine;
using Random = System.Random;

namespace LoadingScreen
{
    public class EventHandlers : IEventHandlerPlayerJoin, IEventHandlerWaitingForPlayers, IEventHandlerRoundStart, IEventHandlerFixedUpdate
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
            {
                /*Door escape = (Door) ev.Server.Map.GetDoors().First(x => x.Name.Equals("ESCAPE")).GetComponent();
                Quaternion rot = escape.localRot;
                */
                doors = ev.Server.Map.GetDoors()
                    /*.Where(x =>
                {
                    if (x.Name.Equals("SURFACE_GATE"))
                        return false;
                    Quaternion rotation = ((Door) x.GetComponent()).localRot;
                    Quaternion rRotation = new Quaternion(-rotation.x,-rotation.y,rotation.z,rotation.w);
                    bool result = rot == rotation || rot == rRotation;
                    plugin.Info($"{x.Name} : {result}");
                    return result;
                })
                */.ToArray();
            }
            else
                doors = ev.Server.Map.GetDoors().Where(x => plugin.doors.Contains(x.Name.ToUpper())).ToArray();
            
            next = DateTime.MaxValue;
            
            plugin.Info($"Available Doors: {doors.Length}");
            preRound = true;
        }

        public void OnPlayerJoin(PlayerJoinEvent ev)
        {
            if (preRound)
            {
                if(plugin.Server.GetPlayers().Count <= 1)
                    next = DateTime.Now.AddSeconds(plugin.seconds);
                if (ev.Player.TeamRole.Team == Smod2.API.Team.NONE)
                {
                    Smod2.API.Door door = getRndDoor();
                    ev.Player.Teleport(door.Position + Vector.One);
                }

            }
        }

        public void OnRoundStart(RoundStartEvent ev)
        {
            preRound = false;
        }

        public Smod2.API.Door getRndDoor()
        {
            return doors[rnd.Next(0, doors.Length)];
        }

        private DateTime next;
        public void OnFixedUpdate(FixedUpdateEvent ev)
        {
            if (preRound && next < DateTime.Now)
            {
                if (plugin.Server.GetPlayers().Count > 0)
                {
                    foreach (var player in plugin.Server.GetPlayers())
                    {
                        if (player.TeamRole.Team == Smod2.API.Team.NONE)
                        {
                            Smod2.API.Door door = getRndDoor();
                            player.Teleport(door.Position + Vector.One);
                        }
                    }
                    next = DateTime.Now.AddSeconds(plugin.seconds);
                }
                else
                {
                    next = DateTime.MaxValue;
                }
            }
        }
    }
}
