using System.Collections;
using DG.Tweening;
using UnityEngine;
using MobileTools;

namespace Game
{
    public class ToggleUITween : MonoBehaviour
    {
        public DOTweenAnimation musicToggleDoTweenAnimation;
        public DOTweenAnimation sfxToggleDoTweenAnimation;

        private void OnEnable()
        {
            StartCoroutine(CheckToggles());
        }

        public void OnValueChange()
        {
            StartCoroutine(CheckToggles());
        }

        private IEnumerator CheckToggles()
        {
            yield return new WaitForSeconds(0);

            if (GameSaves.Instance.Value.MusicIsOn)
                musicToggleDoTweenAnimation.DOPlayForward();
            else
                musicToggleDoTweenAnimation.DOPlayBackwards();

            if (GameSaves.Instance.Value.SfxIsOn)
                sfxToggleDoTweenAnimation.DOPlayForward();
            else
                sfxToggleDoTweenAnimation.DOPlayBackwards();
        }
    }
}