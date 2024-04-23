using UnityEngine;

namespace MobileTools
{
    public class IAPRemoveAdsHandler : MonoBehaviour
    {
        public void RemoveAds()
        {
            if (InAppPurchaseManager.Instance != null)
                InAppPurchaseManager.Instance.RemoveAds();
        }
    }
}