using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Game
{
    public class PlayButtonIdleTween : MonoBehaviour,IPointerUpHandler
    {
        Button m_Button;
        [SerializeField] private float idlePlayDelayTime = 2;
        private Vector3 punch = new Vector3(-0.03f, -0.03f, -0.03f);
        private Vector3 initScale;

        [Header("Events")]
        public UnityEvent OnTweenComplete;

        private void Start()
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(HandleButtonClick);
            initScale = m_Button.transform.localScale;

            InvokeRepeating("PlayIdleTween", idlePlayDelayTime, idlePlayDelayTime);
        }

        private void HandleButtonClick()
        {
            CancelInvoke();
            m_Button.transform.localScale = initScale;

            transform.DOPunchScale(punch, 0.3f, 1, 1).SetEase(Ease.InOutElastic).OnComplete(delegate
            {
                OnTweenComplete?.Invoke();
            });
        }

        private void PlayIdleTween()
        {
            m_Button.transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0.1f), 0.5f, 7, 1)                
                .SetEase(Ease.InOutElastic);
        }

        private void OnDestroy()
        {
            CancelInvoke();
        }        

        public void OnPointerUp(PointerEventData eventData)
        {
            InvokeRepeating("PlayIdleTween", idlePlayDelayTime, idlePlayDelayTime);
        }
    }
}