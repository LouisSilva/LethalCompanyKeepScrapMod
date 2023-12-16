using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalCompanyKeepScrap.Patches;

namespace LethalCompanyKeepScrap
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private const string modGUID = "DarthFigo.LCM_KeepScrap";
        private const string modName = "DarthFigo Lethal Company Keep Scrap Mod";
        private const string modVersion = "1.0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);
        private static Plugin Instance;
        private ManualLogSource mls;
        
        public void Awake()
        {
            if (Instance == null)
            {
                Instance = null;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
            
            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(DespawnPropsAtEndOfRoundPatch));
            harmony.PatchAll(typeof(FillEndGameStatsPatch));
        }
    }
}