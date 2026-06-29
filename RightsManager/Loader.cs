using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using UnityEngine;
using HarmonyLib;
namespace RightsManager
{
    [BepInPlugin(Data.GUID, Data.Name, Data.Version)]
    [BepInDependency("nickklmao.menulib")]
    internal class Loader : BaseUnityPlugin
    {
        internal static ConfigEntry<KeyCode> menuKey;
        void Awake()
        {
            Logger.LogInfo("Loading...");
            Logger.LogInfo("Patching...");
            (new Harmony(Data.GUID)).PatchAll();
            Logger.LogInfo("Patched!");
            Logger.LogInfo("Loading configs...");
            menuKey = Config.Bind<KeyCode>("Binds", "Menu", KeyCode.F10);
            Logger.LogInfo("Configs loaded!");
            Logger.LogInfo("Loaded!");
        }
    }
}
