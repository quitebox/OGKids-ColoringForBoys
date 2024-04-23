using System.Collections.Generic;
using MobileTools.Utilities;
using UnityEngine;

namespace MobileTools
{
    public class InAppPurchaseManager : Manager<InAppPurchaseManager>
    {        
        private void Start()
        {
            IAPManager.Instance.InitializeIAPManager(InitializeResultCallback);           
        }

        private void InitializeResultCallback(IAPOperationStatus status, string message, List<StoreProduct> shopProducts)
        {
            if (status == IAPOperationStatus.Success)
            {
                // IAP was successfully initialized
                // Loop through all products
                for (int i = 0; i < shopProducts.Count; i++)
                {
                    if (shopProducts[i].productName == "RemoveAds")
                    {
                        // If active variable is true, means that user has bought that product
                        // So enable access
                        if (shopProducts[i].active)
                            GameSaves.Instance.SetRemoveAds(true);
                    }

                    if (shopProducts[i].productName == "Subscription")
                    {
                        if (shopProducts[i].active)
                            GameSaves.Instance.SetSubscription(true);
                    }

                    if (shopProducts[i].productName == "UnlockFullVersion")
                    {
                        if (shopProducts[i].active)
                            GameSaves.Instance.UnlockFullApp(true);
                    }
                }
            }
            else
            {
                Debug.Log("Error occured " + message);
            }
        }

        public void RemoveAds()
        {
            IAPManager.Instance.BuyProduct(ShopProductNames.RemoveAds, ProductBoughtCallback);
        }

        public void UnlockAll()
        {
            IAPManager.Instance.BuyProduct(ShopProductNames.UnlockFullVersion, ProductBoughtCallback);
        }

        public void Subscribe()
        {
            //IAPManager.Instance.BuyProduct(ShopProductNames.Subscription, ProductBoughtCallback);
        }

        private void ProductBoughtCallback(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                if (product.productName == "RemoveAds")
                    GameSaves.Instance.SetRemoveAds(true);

                if (product.productName == "Subscription")
                    GameSaves.Instance.SetSubscription(true);

                if (product.productName == "UnlockFullVersion")
                    GameSaves.Instance.UnlockFullApp(true);
            }
            else
            {
                Debug.Log("Buy product failed: " + message);
            }
        }

        public void RestorePurchase()
        {
            IAPManager.Instance.RestorePurchases(ProductRestoredCallback);
        }

        private void ProductRestoredCallback(IAPOperationStatus status, string message, StoreProduct product)
        {
            if (status == IAPOperationStatus.Success)
            {
                // Consumable products are not restored                
                if (product.productName == "RemoveAds")
                    GameSaves.Instance.SetRemoveAds(true);

                if (product.productName == "Subscription")
                    GameSaves.Instance.SetSubscription(true);

                if (product.productName == "UnlockFullVersion")
                    GameSaves.Instance.UnlockFullApp(true);
            }
        }
    }
}