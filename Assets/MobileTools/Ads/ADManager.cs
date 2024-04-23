using MobileTools.Utilities;
using UnityEngine;

namespace MobileTools
{
    public class ADManager : Manager<ADManager>
    {
        [SerializeField] private GameObject ConsentPopupUI;
        [SerializeField] private bool m_Debug;

        private void Start()
        {
            // if user consent was set, just initializa the SDK, else request user consent
            if (Advertisements.Instance.UserConsentWasSet())
            {
                Advertisements.Instance.Initialize();
            }
            else
                DisplayUserConsent();

            Advertisements.Instance.debug = m_Debug;
        }

        private void DisplayUserConsent()
        {
#if UNITY_IOS
 Advertisements.Instance.Initialize();
#else
            ConsentPopupUI.SetActive(true);
#endif
        }

        public void SetUserConsent(bool b)
        {
            Advertisements.Instance.SetUserConsent(b);
            Advertisements.Instance.Initialize();
            ConsentPopupUI.SetActive(false);
        }

        public void RemoveAds()
        {
            Advertisements.Instance.RemoveAds(true);
        }

        public void ShowBannerAd(BannerPosition bannerPosition)
        {
            Advertisements.Instance.ShowBanner(bannerPosition, BannerType.SmartBanner);
        }

        public bool IsBannerOnScreen()
        {
            return Advertisements.Instance.IsBannerOnScreen();
        }

        public void HideBannerAd()
        {
            Advertisements.Instance.HideBanner();
        }

        public void ShowInterstitialAd()
        {
            Advertisements.Instance.ShowInterstitial(InterstitialClosed);
        }

        public void ShowRewardedVideo()
        {
            Advertisements.Instance.ShowRewardedVideo(CompleteMethod);
        }

        //callback called each time an interstitial is closed
        private void InterstitialClosed(string advertiser)
        {
            if (Advertisements.Instance.debug)
            {
                Debug.Log("Interstitial closed from: " + advertiser + " -> Resume Game ");
                GleyMobileAds.ScreenWriter.Write("Interstitial closed from: " + advertiser + " -> Resume Game ");
            }
        }

        //callback called each time a rewarded video is closed
        //if completed = true, rewarded video was seen until the end
        private void CompleteMethod(bool completed, string advertiser)
        {
            if (Advertisements.Instance.debug)
            {
                Debug.Log("Closed rewarded from: " + advertiser + " -> Completed " + completed);
                GleyMobileAds.ScreenWriter.Write("Closed rewarded from: " + advertiser + " -> Completed " + completed);
                if (completed == true)
                {
                    //give the reward
                }
                else
                {
                    //no reward
                }
            }
        }
    }
}