using UnityEngine;

namespace DVModApi
{
    class DVMODInit : MonoBehaviour
    {
        public DVApi_LoadingScreen loadingScreenEvents;

        void FixedUpdate()
        {
            //Wait for loading screen and destroy itself.
            if (GameObject.Find("loading screen GUI") != null)
            {
                GameObject.Find("loading screen GUI").AddComponent<DVMODCheckLoading>().loadingScreenEvents = loadingScreenEvents;
                Destroy(gameObject);
            }
        }
    }
}
