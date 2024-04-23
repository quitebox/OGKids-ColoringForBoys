using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTools
{
    public class DestroyBannerADView : MonoBehaviour
    {
        private void Start()
        {
            if (ADManager.Instance == null)
                return;

            if (ADManager.Instance.IsBannerOnScreen())
                ADManager.Instance.HideBannerAd();
        }
    }
}
