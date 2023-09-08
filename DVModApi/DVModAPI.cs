using System.IO;
using System;
using UnityEngine;
using UnityModManagerNet;
using System.Collections.Generic;
using System.Linq;

namespace DVModApi
{
    public enum FunctionType
    {
        /// <summary>
        /// OnMenuLoad - Called once when Main Menu has been fully loaded (Happens after UMM's "Load" entry point)
        /// </summary>
        OnMenuLoad,
        /// <summary>
        /// OnGameLoad - Called once when game has been loaded (After Loading Screen)
        /// </summary>
        OnGameLoad,
        /// <summary>
        /// OnSave - Executed once when game is being saved.
        /// </summary>
        OnSave,
        /// <summary>
        /// ModSettings - ModAPI Settings should be created here. (Created settings are visible only in "Better Mod UI", this is NOT for UMM settings)
        /// </summary>
        ModSettings,
        /// <summary>
        /// ModSettingsLoaded - Called after saved settings data have been loaded from file. (ModAPI Settings, this is NOT for UMM settings)
        /// </summary>
        ModSettingsLoaded
    }

    internal class DVModEntry
    {
        internal UnityModManager.ModEntry modEntry;

        internal Action<bool> A_OnMenuLoad;     //Load in main menu
        internal Action A_OnGameLoad;           //When Game is loaded
        internal Action A_OnSave;               //When Game is saved
        internal Action A_ModSettings;          //Settings Creation  
        internal Action A_ModSettingsLoaded;    //When mod settings have been loaded from file

        internal bool hasModSettings = false;
        internal List<ModSettings> modSettings; //List of custom Mod Settings

        internal DVModEntry(UnityModManager.ModEntry m)
        {
            modEntry = m;
        }
        internal void ModSettingsSetup()
        {
            hasModSettings = true;
            modSettings = new List<ModSettings>();
        }
        internal void SetupBoolFunction(FunctionType functionType, Action<bool> function)
        {
            if (functionType == FunctionType.OnMenuLoad)
                if (A_OnMenuLoad == null)
                    A_OnMenuLoad = function;
                else
                    LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. You already created <color=#00ffffff>OnMenuLoad</color> function type.");
            else
            {
                LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. Invalid action for {functionType}.");
                throw new Exception($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. Invalid action for {functionType}.");
            }
        }
        internal void SetupFunction(FunctionType functionType, Action function)
        {
            switch (functionType)
            {
                case FunctionType.OnMenuLoad:
                    LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. <color=#00ffffff>OnMenuLoad</color> must be Action<bool>.");
                    throw new Exception($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. <color=#00ffffff>OnMenuLoad</color> must be Action<bool>.");
                case FunctionType.OnGameLoad:
                    if (A_OnGameLoad == null)
                        A_OnGameLoad = function;
                    else
                        LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. You already created <color=#00ffffff>OnGameLoad</color> function type.");
                    break;
                case FunctionType.OnSave:
                    if (A_OnSave == null)
                        A_OnSave = function;
                    else
                        LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. You already created <color=#00ffffff>OnSave</color>> function type.");
                    break;
                case FunctionType.ModSettings:
                    if (A_ModSettings == null)
                        A_ModSettings = function;
                    else
                        LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. You already created <color=#00ffffff>ModSettings</color> function type.");
                    break;
                case FunctionType.ModSettingsLoaded:
                    if (A_ModSettingsLoaded == null)
                        A_ModSettingsLoaded = function;
                    else
                        LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. You already created <color=#00ffffff>ModSettingsLoaded</color> function type.");
                    break;
                default:
                        LogHelper.LogError($"Setup() error for <color=#00ffffff>{modEntry.Info.Id}</color>. There is no such function (are you on outdated version of API? Also don't bitwise it)");
                    break;
            }
        }
    }

    public class DVModAPI
    {
        internal static List<DVModEntry> DVModEntries = new List<DVModEntry>();
        internal static GameObject DVModAPIGO;

        /// <summary>
        /// Setup functions controlled by DVModAPI
        /// </summary>
        /// <param name="modEntry">your UMM modEntry</param>
        /// <param name="functionType">functionType to Setup</param>
        /// <param name="function">Actual function to execute</param>
        public static void Setup(UnityModManager.ModEntry modEntry, FunctionType functionType, Action function)
        {
            DVModEntry me = DVModEntries.Where(x => x.modEntry == modEntry).FirstOrDefault();   
            if (me == null)
            {
                me = new DVModEntry(modEntry);
                DVModEntries.Add(me);
            }
            me.SetupFunction(functionType, function);
        }
        public static void Setup(UnityModManager.ModEntry modEntry, FunctionType functionType, Action<bool> function)
        {
            DVModEntry me = DVModEntries.Where(x => x.modEntry == modEntry).FirstOrDefault();
            if (me == null)
            {
                me = new DVModEntry(modEntry);
                DVModEntries.Add(me);
            }
            me.SetupBoolFunction(functionType, function);
        }

        internal static void Init()
        {
            DVModEntries.Clear();
        }       
    }
}
