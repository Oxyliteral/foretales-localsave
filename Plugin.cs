using System;
using System.Runtime.CompilerServices;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using Common.Data;
using Common.HomeScreen.Presenter;
using Common.HomeScreen.View;
using Egs;
using Epic.OnlineServices;
using Epic.OnlineServices.PlayerDataStorage;
using Epic.OnlineServices.Stats;
using HarmonyLib;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppSystem.Threading.Tasks;
using Scripts;

namespace MyFirstPlugin;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class Plugin : BasePlugin
{
    internal static new ManualLogSource Log;

    public override void Load()
    {
        Log = base.Log;
        Harmony.CreateAndPatchAll(this.GetType());
        Log.LogWarning($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
    }
    [HarmonyPatch(typeof(OnQueryStatsCompleteCallbackInternal), "Invoke"), HarmonyPrefix]
	static void HarmonyPatch_OnQueryStatsCompleteCallbackInfoInternal_Invoke_Prefix(ref OnQueryStatsCompleteCallbackInfoInternal data)
	{
		data.ResultCode = Result.Success;
	}

	[HarmonyPatch(typeof(OnQueryStatsCompleteCallbackInternal), "Invoke"), HarmonyPostfix]
    static void HarmonyPatch_OnQueryStatsCompleteCallbackInfoInternal_Invoke_Postfix(ref OnQueryStatsCompleteCallbackInfoInternal data)
	{
		data.ResultCode = Result.Success;
	}
	/*
	[HarmonyPatch(typeof(Il2CppException), "RaiseExceptionIfNecessary"), HarmonyPrefix]
	static bool HarmonyPatch_Il2CppException_RaiseExceptionIfNecessary_Prefix(IntPtr returnedException)
	{
		return false;
	}
	*/
	static LocalFileDataStorage storage;

	[HarmonyPatch(typeof(EgsCloudDataStorage), "Save"), HarmonyPrefix]
	static bool HarmonyPatch_EgsCloudDataStorage_Save_Prefix(string persistenceId, Il2CppStructArray<byte> bytes, ref Task __result)
	{
		var starter = AppStateController.GetStarter<EngineStarter>();
		//__result = starter._platform.GameDataStorage.Save(persistenceId, bytes);
		if (storage == null)
		{
			storage = new LocalFileDataStorage();
		}
		__result = storage.Save(persistenceId, bytes);
		return false;
	}

	[HarmonyPatch(typeof(EgsCloudDataStorage), "Load"), HarmonyPrefix]
	static bool HarmonyPatch_EgsCloudDataStorage_Load_Prefix(string persistenceId, ref Task __result)
	{
		var starter = AppStateController.GetStarter<EngineStarter>();
		//__result = starter._platform.GameDataStorage.Save(persistenceId, bytes);
		if (storage == null)
		{
			storage = new LocalFileDataStorage();
		}
		__result = storage.Load(persistenceId);
		return false;
	}
}