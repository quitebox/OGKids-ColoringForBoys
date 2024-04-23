using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    [RequireComponent(typeof(CanvasGroup))]
    public class OnEnableFadeInTween : MonoBehaviour
    {
        CanvasGroup m_CanvasGroup;

        [Header("Events")]
        public UnityEvent OnComplete;

        private void OnEnable()
        {
            if (m_CanvasGroup == null)
                m_CanvasGroup = GetComponent<CanvasGroup>();

            m_CanvasGroup.DOFade(0f, 0.6f).From().OnComplete(delegate
            {
                OnComplete?.Invoke();
            });
        }
    }
}