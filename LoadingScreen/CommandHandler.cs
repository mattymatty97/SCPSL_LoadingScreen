using Smod2;
using Smod2.Commands;

namespace LoadingScreen
{
    public class CommandHandler : ICommandHandler
    {
        private readonly EventHandlers handler;
        private readonly Plugin plugin;

        public CommandHandler(EventHandlers handler, Plugin plugin)
        {
            this.handler = handler;
            this.plugin = plugin;
        }

        public string[] OnCall(ICommandSender sender, string[] args)
        {
            handler.TeleportToRandomDoors(plugin.Server.GetPlayers());

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
    }
}
