using UnityEngine;

namespace MobileTools
{
    public class IAPUnlockAllHandler : MonoBehaviour
    {
        public void UnlockAll()
        {
            if (InAppPurchaseManager.Instance != null)
                InAppPurchaseManager.Instance.UnlockAll();
        }
    }
}