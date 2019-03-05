using Smod2;
using Smod2.API;
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
        version = "1.0.0"
            )]
    public class LoadingScreen : Plugin
    {
        public string[] doors;
        public int seconds;
        public EventHandlers Handlers { get; private set; }

        public override void Register()
        {
            
            string[] defaultDoors =
            {
                "*"
            };
            AddConfig(new ConfigSetting("ls_doorId", defaultDoors, SettingType.LIST, true,"list of door ids to use"));
            AddConfig(new ConfigSetting("ls_refresh_time", 15, SettingType.NUMERIC, true,"seconds betwen screen refresh"));

            Handlers = new EventHandlers(this);

            AddEventHandlers(Handlers, Priority.Low);
            AddCommand("ls_refresh",new CommandHandler(Handlers,this));
        }

        public void RefreshConfig()
        {
            doors = GetConfigList("ls_doorId");
            seconds = GetConfigInt("ls_refresh_time");
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
