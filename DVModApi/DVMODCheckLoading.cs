using UnityEngine;

namespace DVModApi
{
    class DVMODCheckLoading : MonoBehaviour
    {
        public DVApi_LoadingScreen loadingScreenEvents;

        //When This Object is destroyed, game finished loading
        void OnDestroy()
        {
            loadingScreenEvents.LoadingScreenFinished();
        }
    }
}
