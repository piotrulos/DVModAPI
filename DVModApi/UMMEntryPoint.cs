using System;
using UnityEngine;
using UnityModManagerNet;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;

namespace DVModApi
{
    public static class UMMEntryPoint
    {
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            LogHelper.Log("<color=#00ffffff>Initializing...</color>");
            DVModAPI.Init();
            if (DVModAPI.DVModAPIGO == null)
            {
                SceneManager.sceneLoaded += SceneManager_sceneLoaded;
                DVModAPI.DVModAPIGO = new GameObject("DVModAPI");
                GameObject.DontDestroyOnLoad(DVModAPI.DVModAPIGO);
                DVModAPI.DVModAPIGO.AddComponent<DVModAPIManager>();
                WorldStreamingInit.LoadingFinished += WorldStreamingInit_LoadingFinished;
                SaveGameManager.AboutToSave += SaveGameManager_AboutToSave;
            }
            LogHelper.Log($"Loaded (version: <color=#00ffffff>{modEntry.Version}</color>)");
            return true;
        }

        private static void SaveGameManager_AboutToSave(DV.Common.SaveType saveType)
        {
            LogHelper.Log("Game is getting saved...");
            LogHelper.Log($"Save type: <color=yellow>{saveType} save</color>");
     
            for (int i = 0; i < DVModAPI.DVModEntries.Count; i++)
            {
                //Call OnSave for registered mods.
                if (DVModAPI.DVModEntries[i].A_OnSave != null)
                {
                    try
                    {
                        LogHelper.Log($"Loading <color=#00ffffff>OnSave</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>");
                        DVModAPI.DVModEntries[i].A_OnSave.Invoke();
                    }
                    catch (Exception e)
                    {
                        LogHelper.LogError($"Failed during Loading <color=#00ffffff>OnSave</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>{Environment.NewLine}DETAILS: {e.Message}");
                        Debug.LogError(e);
                    }
                }
            }
        }

        private static void WorldStreamingInit_LoadingFinished()
        {
            LogHelper.Log("Game has been loaded");

            for (int i = 0; i < DVModAPI.DVModEntries.Count; i++)
            {
                //Call OnGameLoad for registered mods.
                if (DVModAPI.DVModEntries[i].A_OnGameLoad != null)
                {
                    try
                    {
                        LogHelper.Log($"Loading <color=#00ffffff>OnGameLoad</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>");
                        DVModAPI.DVModEntries[i].A_OnGameLoad.Invoke();
                    }
                    catch (Exception e)
                    {
                        LogHelper.LogError($"Failed during Loading <color=#00ffffff>OnGameLoad</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>{Environment.NewLine}DETAILS: {e.Message}");
                        Debug.LogError(e);
                    }
                }
            }
        }
        private static void SceneManager_sceneLoaded(Scene scene, LoadSceneMode sceneMode)
        {
            if (scene.name == "MainMenu_LFS")
            {
                LogHelper.Log("Main Menu has been fully loaded");
                for (int i = 0; i < DVModAPI.DVModEntries.Count; i++)
                {
                    //Call ModSettings for registered mods.
                    if (DVModAPI.DVModEntries[i].A_ModSettings != null)
                    {
                        try
                        {
                            DVModSettings.Settings(DVModAPI.DVModEntries[i]);
                            DVModAPI.DVModEntries[i].A_ModSettings.Invoke();
                        }
                        catch (Exception e)
                        {
                            LogHelper.LogError($"Failed during Creating <color=#00ffffff>ModSettings</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>{Environment.NewLine}DETAILS: {e.Message}");
                            Debug.LogError(e);
                        }
                    }

                    //Call OnMenuLoad for registered mods.
                    if (DVModAPI.DVModEntries[i].A_OnMenuLoad != null)
                    {
                        try
                        {
                            LogHelper.Log($"Loading <color=#00ffffff>OnMenuLoad</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>");
                            DVModAPI.DVModEntries[i].A_OnMenuLoad.Invoke();
                        }
                        catch (Exception e)
                        {
                            LogHelper.LogError($"Failed during Loading <color=#00ffffff>OnMenuLoad</color> for <color=#00ffffff>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>{Environment.NewLine}DETAILS: {e.Message}");
                            Debug.LogError(e);
                        }
                    }
                }
            }
        }

    }
}
