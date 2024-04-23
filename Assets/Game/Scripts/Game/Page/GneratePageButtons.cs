using System.Collections.Generic;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class GneratePageButtons : MonoBehaviour
    {
        [SerializeField] private List<PageConfig> m_Pages;
        [SerializeField] private ColorPageButton m_ColorPageButton;
        [SerializeField] private RectTransform m_ButtonsParent;
        //[SerializeField] private ScrollRect m_ScrollRect;
        [SerializeField] private int pageCountRequiredForReview = 3;


        [Space]
        [Header("Events")]
        public UnityEvent OnPageSelect;
        public UnityEvent OnLoadAdRequest;
        public UnityEvent OnRemoveAdRequest;

        public static bool isAdRequest = false;
        private static int pageSelectionCount = 0;
        private static int requiredPageSelectionCountToRequestAd = 3;
        private static int selectedPageCount = 0;
        private static int requiredPageSelectionCountToRequestRemoveAds = 4;

        private void Start()
        {
            CheckForReview();
            GenerateButtons();
        }

        private void CheckForReview()
        {
            if (selectedPageCount >= pageCountRequiredForReview)
                if (RateGame.Instance != null)
                {
                    RateGame.Instance.ShowRatePopup();
                    selectedPageCount = 0;
                }
        }

        private void GenerateButtons()
        {
            for (int i = 0; i < m_Pages.Count; i++)
            {
                ColorPageButton colorPageButton = Instantiate(m_ColorPageButton, m_ButtonsParent);
                colorPageButton.gameObject.SetActive(true);
                colorPageButton.m_PageConfig = m_Pages[i];
                PageConfig page = m_Pages[i];
                colorPageButton.name = page.name;

                int pageIndex = i;
                //colorPageButton.Button.onClick.AddListener(delegate
                //{
                //    AppData.SelectedPageConfig = page;
                //    pageSelectionCount++;
                //    selectedPageCount++;
                //    OnPageSelect?.Invoke();
                //});

                colorPageButton.UpdateUI();

                //yield return null;
            }
        }

        private void CheckForAds()
        {
            //if (AdsManager.Instance == null)
            //{
            //    Debug.Log($"[GeneratePageButtons]: Not found Ads Manager");
            //    return;
            //}

            //if (!AdsManager.Instance.CanShowInterstialAd)
            //{
            //    Debug.Log($"[GeneratePageButtons]: Ad is not ready");
            //    return;
            //}

            //Debug.Log($"[GeneratePageButtons]: Checking for ads...");
            //Debug.Log($"[GeneratePageButtons]: Page selection is {pageSelectionCount}");

            //if (pageSelectionCount >= requiredPageSelectionCountToRequestAd)
            //{
            //    isAdRequest = true;

            //    OnLoadAdRequest?.Invoke();

            //    if (pageSelectionCount >= requiredPageSelectionCountToRequestRemoveAds)
            //    {
            //        Debug.Log($"[GeneratePageButtons]: Requesting to remove ads");

            //        pageSelectionCount = 0;
            //        isAdRequest = false;

            //        OnRemoveAdRequest?.Invoke();
            //    }
            //}
        }
    }
}