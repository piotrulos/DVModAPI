using System.IO;
using System;
using UnityEngine;
using UnityModManagerNet;

namespace DVModApi
{
    public delegate void DVisLoaded(); 
    public static class DVModAPI
    {
        public static DVAPI_LoadingScreen DVLoadingScreen;
        private static string DVModAPIVersion = "0.1";
        static bool Load(UnityModManager.ModEntry modEntry)
        {
            Debug.Log("<color=cyan>[DVModAPI] Initializing...</color>");
            DVLoadingScreen = new DVAPI_LoadingScreen();
            GameObject init = new GameObject("DVModApi");
            GameObject.DontDestroyOnLoad(init);
            WorldStreamingInit.LoadingFinished += WorldStreamingInit_LoadingFinished;
            DVLoadingScreen.LoadingFinished += DVLoadingScreen_LoadingFinished;
            Debug.Log($"<color=cyan>[DVModAPI] Loaded (version: {DVModAPIVersion})</color>");
            return true;
        }

        private static void DVLoadingScreen_LoadingFinished()
        {
            Debug.Log("<color=cyan>[DVModAPI] Game Loaded</color>");
        }

        private static void WorldStreamingInit_LoadingFinished()
        {
            DVLoadingScreen.LoadingScreenFinished();
        }

               
        /// <summary>
        /// Load Asset bundle from your mod folder
        /// </summary>
        /// <param name="modEntryPath"></param>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        /// <exception cref="FileNotFoundException"></exception>
        public static AssetBundle LoadAssetBundle(string modEntryPath, string bundleName)
        {
            string bundle = Path.Combine(modEntryPath, bundleName);

            if (File.Exists(bundle))
            {
                Debug.Log($"Loading AssetBundle: {bundle}...");
                return AssetBundle.LoadFromMemory(File.ReadAllBytes(bundle));
            }
            else
            {
                throw new FileNotFoundException($"Error: File not found: {bundle} {Environment.NewLine}", bundle);
            }
        }
    }

    public class DVAPI_LoadingScreen
    {
        public event DVisLoaded LoadingFinished;
        internal void LoadingScreenFinished()
        {
            OnLoadingScreenFinished();
        }
        protected virtual void OnLoadingScreenFinished()
        {
            LoadingFinished?.Invoke();
        }
    }

}
