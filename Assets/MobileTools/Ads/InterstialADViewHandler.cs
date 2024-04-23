using UnityEngine;

namespace MobileTools
{
    public class InterstialADViewHandler : MonoBehaviour
    {
        private static int playAdAfterIfSceneLoadCount = 0;
        [SerializeField] private int maxSceneLoadCount = 0;

        [SerializeField] private string lastAdTimeKey = "LastAdTime";
        private float adInterval = 0f;

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

            playAdAfterIfSceneLoadCount++;

            if (playAdAfterIfSceneLoadCount >= maxSceneLoadCount)
            {
                CheckPlayAd();
            }
        }

        public void PlayAd()
        {
            // Update the last ad time
            ADManager.Instance.ShowInterstitialAd();
            PlayerPrefs.SetFloat(lastAdTimeKey, Time.time);
        }

        private void CheckPlayAd()
        {
            adInterval = 180f;

            // Get the last ad time from PlayerPrefs
            float lastAdTime = PlayerPrefs.GetFloat(lastAdTimeKey, 0f);

            // Check if enough time has passed since the last ad was played
            if (Time.time - lastAdTime >= adInterval)
            {
                // Play the ad
                PlayAd();
            }
            else
            {
                // Calculate the remaining time until the next ad can be played
                float timeUntilNextAd = adInterval - (Time.time - lastAdTime);
                Debug.Log("Next ad can be played in " + Mathf.CeilToInt(timeUntilNextAd) + " seconds.");
            }
        }

    }
}
