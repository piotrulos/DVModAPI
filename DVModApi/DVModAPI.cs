using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityModManagerNet;

namespace DVModApi
{
    public delegate void DVisLoaded();  // delegate
    public static class DVModAPI
    {
        public static DVApi_LoadingScreen DVLoadingScreen;

        static bool Load(UnityModManager.ModEntry modEntry)
        {
            DVLoadingScreen = new DVApi_LoadingScreen();
            GameObject init = new GameObject("DVModApi - init");
            init.AddComponent<DVMODInit>().loadingScreenEvents = DVLoadingScreen;
            GameObject.DontDestroyOnLoad(init);
            DVLoadingScreen.loadingScreenFinished += DVLoadingScreen_loadingScreenFinished;
            return true;
        }

        private static void DVLoadingScreen_loadingScreenFinished()
        {
            Debug.Log("<color=cyan>[DVModAPI] Game Loaded</color>");

        }
    }

    public class DVApi_LoadingScreen
    {
        public event DVisLoaded loadingScreenFinished;
        internal void LoadingScreenFinished()
        {
            OnLoadingScreenFinished();
        }
        protected virtual void OnLoadingScreenFinished()
        {
            loadingScreenFinished?.Invoke();
        }
    }

}
