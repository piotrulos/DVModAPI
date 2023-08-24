using System.IO;
using System;
using UnityEngine;
using UnityModManagerNet;
using UnityEngine.SceneManagement;

namespace DVModApi
{
    public static class UMMEntryPoint
    {
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            LogHelper.Log("<color=cyan>Initializing...</color>");
            DVModAPI.Init();
            if (DVModAPI.DVModAPIGO == null)
            {
                SceneManager.sceneLoaded += SceneManager_sceneLoaded;
                DVModAPI.DVModAPIGO = new GameObject("DVModAPI");
                GameObject.DontDestroyOnLoad(DVModAPI.DVModAPIGO);
                DVModAPI.DVModAPIGO.AddComponent<DVModAPIManager>();
                WorldStreamingInit.LoadingFinished += WorldStreamingInit_LoadingFinished;
            }
            LogHelper.Log($"Loaded (version: <color=cyan>{modEntry.Version}</color>)");
            return true;
        }
        private static void WorldStreamingInit_LoadingFinished()
        {
            LogHelper.Log("Game has been loaded");
            //Call OnGameLoad for registered mods.
            for (int i = 0; i < DVModAPI.DVModEntries.Count; i++)
            {
                if (DVModAPI.DVModEntries[i].A_OnGameLoad != null)
                {
                    try
                    {
                        LogHelper.Log($"Loading <color=cyan>OnGameLoad</color> for <color=cyan>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>");
                        DVModAPI.DVModEntries[i].A_OnGameLoad.Invoke();
                    }
                    catch (Exception e)
                    {
                        LogHelper.LogError($"Failed during Loading <color=cyan>OnGameLoad</color> for <color=cyan>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>{Environment.NewLine}DETAILS: {e.Message}");
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
                //Call OnMenuLoad for registered mods.
                for (int i = 0; i < DVModAPI.DVModEntries.Count; i++)
                {
                    if (DVModAPI.DVModEntries[i].A_OnMenuLoad != null)
                    {
                        try
                        {
                            LogHelper.Log($"Loading <color=cyan>OnMenuLoad</color> for <color=cyan>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>");
                            DVModAPI.DVModEntries[i].A_OnMenuLoad.Invoke();
                        }
                        catch (Exception e)
                        {
                            LogHelper.LogError($"Failed during Loading <color=cyan>OnMenuLoad</color> for <color=cyan>{DVModAPI.DVModEntries[i].modEntry.Info.Id}</color>{Environment.NewLine}DETAILS: {e.Message}");
                            Debug.LogError(e);
                        }
                    }
                }
            }
        }

    }
}
