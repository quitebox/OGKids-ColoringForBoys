using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTools
{
    public class RemoveAdsHandler : MonoBehaviour
    {
        public void RemoveAds()
        {
            if (ADManager.Instance != null)
                ADManager.Instance.RemoveAds();
        }
    }
}