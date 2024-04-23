using UnityEngine;

namespace MobileTools
{
    public class IAPRestorePurchaseHandler : MonoBehaviour
    {
        public void RestorePurchase()
        {
            if (InAppPurchaseManager.Instance != null)
                InAppPurchaseManager.Instance.RemoveAds();            
        }
    }
}