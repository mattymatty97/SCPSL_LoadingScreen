using Smod2;
using Smod2.API;
using Smod2.Commands;

namespace LoadingScreen
{
    public class CommandHandler : ICommandHandler
    {
        private EventHandlers Handler;
        private Plugin Plugin;
        public string[] OnCall(ICommandSender sender, string[] args)
        {
            foreach (var player in Plugin.Server.GetPlayers())
            {
                if (player.TeamRole.Team == Smod2.API.Team.NONE)
                {
                    Smod2.API.Door door = Handler.getRndDoor();
                    player.Teleport(door.Position + Vector.One);
                }
            }

            return new[] {"All players screen refreshed"};
        }

        public string GetUsage()
        {
            return "ls_refresh";
        }

        public string GetCommandDescription()
        {
            return "Refresh loading screen for all connected players";
        }

        public CommandHandler(EventHandlers handler, Plugin plugin)
        {
            Handler = handler;
            Plugin = plugin;
        }
    }
}