using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class CategoryScrollSnapAnimator : MonoBehaviour
    {
        [SerializeField] private SimpleScrollSnap simpleScrollSnap;

        private void OnEnable()
        {
            StartCoroutine(StartRoutine());
        }

        private IEnumerator StartRoutine()
        {
            simpleScrollSnap.GoToPanel(PlayerPrefs.GetInt("CategoriesStartingPanel", 0));

            yield return new WaitForSeconds(1f);

            simpleScrollSnap.OnPanelCentering.AddListener(OnPanelCentering);

            AnimateCenteredPanel();
            AnimateOtherPanels();
        }

        private void OnPanelCentering(int arg0, int arg1)
        {
            AnimateCenteredPanel();
            AnimateOtherPanels();
        }

        private void AnimateOtherPanels()
        {
            for (int i = 0; i < simpleScrollSnap.Panels.Length; i++)
                if (i != simpleScrollSnap.CenteredPanel)
                    simpleScrollSnap.Panels[i].transform.GetChild(0).DOScale(new Vector3(1f, 1f, 1f), 0.1f).SetEase(Ease.Linear);
        }

        private void AnimateCenteredPanel()
        {
            AnimateOtherPanels();
            simpleScrollSnap.Panels[simpleScrollSnap.CenteredPanel].transform.GetChild(0).DOScale(new Vector3(1.1f, 1.1f, 1.1f), 1f).SetEase(Ease.OutElastic);

            PlayerPrefs.SetInt("CategoriesStartingPanel", simpleScrollSnap.CenteredPanel);
            PlayerPrefs.Save();
        }
    }
}
