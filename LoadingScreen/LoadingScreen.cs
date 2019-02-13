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
        SmodMinor = 2,
        SmodRevision = 0,
        version = "0.0.1"
            )]
    public class LoadingScreen : Plugin
    {
        public string[] doors;
        public EventHandlers Handlers { get; private set; }

        public override void Register()
        {
            
            string[] defaultDoors =
            {
                "*"
            };
            AddConfig(new ConfigSetting("ls_doorId", defaultDoors, SettingType.LIST, true,
                "Ranks allowed to adjust player Preferences."));

            Handlers = new EventHandlers(this);

            AddEventHandlers(Handlers, Priority.Low);
        }

        public void RefreshConfig()
        {
            doors = GetConfigList("ls_doorId");
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
