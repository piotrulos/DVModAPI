using DV.UI;
using DV.UIFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DVModApi
{
    internal class DVModAPIManager : MonoBehaviour
    {
        private PopupManager popupManager;
        public void Start()
        {
            this.FindPopupManager(ref popupManager);            
        }
    }
}
