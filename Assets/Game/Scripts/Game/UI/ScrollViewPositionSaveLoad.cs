using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class ScrollViewPositionSaveLoad : MonoBehaviour, IPointerEnterHandler
    {
        RectTransform m_RectTransform;

        private Vector3 positionHistory;
        private Vector3 newPosition;

        private void Awake()
        {
            m_RectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            positionHistory = m_RectTransform.localPosition;
            positionHistory.y = PlayerPrefs.GetFloat(gameObject.name, 0f);
            m_RectTransform.localPosition = positionHistory;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            newPosition = m_RectTransform.localPosition;
            PlayerPrefs.SetFloat(gameObject.name, newPosition.y);
            PlayerPrefs.Save();

            Debug.Log("Saving scroll position");
        }
    }
}