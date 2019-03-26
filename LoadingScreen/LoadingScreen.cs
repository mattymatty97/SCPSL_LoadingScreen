using System.Linq;
using Smod2;
using Smod2.Attributes;
using Smod2.Config;
using Smod2.Events;

namespace LoadingScreen
{
    [PluginDetails(
        name = "Loading Screen",
        author = "The Matty",
        description = "Improves \"waiting for players\" Screen by teleporting people to random doors",
        id = "mattymatty.loadingscreen",
        SmodMajor = 3,
        SmodMinor = 3,
        SmodRevision = 0,
        version = "1.1.0"
    )]
    public class LoadingScreen : Plugin
    {
        public string[] doors;
        public int seconds;
        public EventHandlers Handlers { get; private set; }

        public const string DoorIdKey = "ls_doorId";
        public const string RefreshTimeKey = "ls_refresh_time";

        public override void Register()
        {
            AddConfig(new ConfigSetting(DoorIdKey, new string[] {"*"}, SettingType.LIST, true, "List of door IDs to use"));
            AddConfig(new ConfigSetting(RefreshTimeKey, 15, SettingType.NUMERIC, true, "Number of seconds between each screen refresh"));

            Handlers = new EventHandlers(this);

            AddEventHandlers(Handlers, Priority.Low);
            AddCommand("ls_refresh", new CommandHandler(Handlers, this));
        }

        public void RefreshConfig()
        {
            doors = ArrayToUpper(GetConfigList(DoorIdKey));
            seconds = GetConfigInt(RefreshTimeKey);
        }

        private static string[] ArrayToUpper(string[] inArray)
        {
            return inArray.Select(index => index.ToUpper()).ToArray();
        }

        public override void OnEnable()
        {
            Info("Loading Screen enabled!");
        }

        public override void OnDisable()
        {
            Info("Loading Screen disabled!");
        }
    }
}
