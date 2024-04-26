using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTools
{
    public class WebviewHandler : MonoBehaviour
    {
        public void LoadURL(string URL)
        {
            //if (WebviewManager.Instance != null)
            //    WebviewManager.Instance.LoadURL(URL);

            Application.OpenURL(URL);
        }
    }
}