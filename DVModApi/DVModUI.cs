using DV.UI;
using DV.UIFramework;
using System;
using UnityEngine;

namespace DVModApi
{
    /// <summary>
    /// UI related API
    /// </summary>
    public class DVModUI
    {
        private static PopupNotificationReferences pnr;
        internal static void GetPopupReferences(PopupNotificationReferences popup)
        {
            pnr = popup;
        }
        /// <summary>
        /// Shows Popup message with just one button called "confirm"
        /// </summary>
        /// <param name="message">A message to display</param>
        /// <param name="ifClosed">(Optional) if popup is closed by clicking button</param>
        public static void ShowPopupMessage(string message, Action ifClosed = null)
        {
            if (pnr != null)
            {
                if (pnr.popupOk == null)
                {
                    LogHelper.LogError("Couldn't get popupOk prefab");
                    return;
                }
                GameObject messageBox = GameObject.Instantiate(pnr.popupOk.gameObject, pnr.popupManager.popupTargetCanvas.transform);
                Popup popup = messageBox.GetComponent<Popup>();
                popup.labelTMPro.text = message;
                popup.positiveButton.onClick.AddListener(delegate
                {
                    ifClosed?.Invoke();
                    DestroyPopup(messageBox);
                });
            }
        }
        /// <summary>
        /// Shows 2 button popup (confirm, cancel)
        /// </summary>
        /// <param name="message">A message to display</param>
        /// <param name="ifConfirm">What to do if confirm button was clicked</param>
        public static void ShowYesNoMessage(string message, Action ifConfirm)
        {
            if (pnr != null)
            {
                if (pnr.popupYesNo == null) 
                {
                    LogHelper.LogError("Couldn't get popupYesNo prefab");
                    return;
                }
                GameObject messageBox = GameObject.Instantiate(pnr.popupYesNo.gameObject, pnr.popupManager.popupTargetCanvas.transform);
                Popup popup = messageBox.GetComponent<Popup>();
                popup.labelTMPro.text = message;
                popup.negativeButton.onClick.AddListener(delegate
                {
                    DestroyPopup(messageBox);
                });
                popup.positiveButton.onClick.AddListener(delegate
                {
                    ifConfirm?.Invoke();
                    DestroyPopup(messageBox);
                });
            }
        }
        internal static void DestroyPopup(GameObject popup)
        {
            GameObject.Destroy(popup);
        }
    }
}
