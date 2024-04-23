using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

namespace Game
{
    public class UIButtonPressedTween : MonoBehaviour
    {
        Button m_Button;

        [Header("Events")]
        public UnityEvent OnTweenComplete;

        private Vector3 punch = new Vector3(-0.03f, -0.03f, -0.03f);

        private void Start()
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(HandleButtonClick);
        }

        private void HandleButtonClick()
        {
            transform.DOPunchScale(punch, 0.3f, 1, 1).SetEase(Ease.InOutElastic).OnComplete(delegate
            {
                OnTweenComplete?.Invoke();
            });
        }
    }
}