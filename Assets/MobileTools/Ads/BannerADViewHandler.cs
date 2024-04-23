using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTools
{
    public class BannerADViewHandler : MonoBehaviour
    {
        public BannerPosition m_BannerPosition = BannerPosition.BOTTOM;

        private void Start()
        {
            if (ADManager.Instance == null)
                return;

            if (GameSaves.Instance == null)
                return;

            if (GameSaves.Instance.Value.HasRemoveAds)
                return;

            if (ADManager.Instance.IsBannerOnScreen())
                ADManager.Instance.HideBannerAd();
            else
                ADManager.Instance.ShowBannerAd(m_BannerPosition);
        }
    }
}
