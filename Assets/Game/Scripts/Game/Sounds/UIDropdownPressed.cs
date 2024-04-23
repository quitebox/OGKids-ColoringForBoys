using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Game
{
    public class UIDropdownPressed : MonoBehaviour, IPointerDownHandler
    {
        Dropdown m_Dropdown;
        [SerializeField] SoundManager.SFX_TYPE m_Type;

        public void OnPointerDown(PointerEventData eventData)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.Play(m_Type);
        }

        private void Start()
        {
            m_Dropdown = GetComponent<Dropdown>();
            m_Dropdown.onValueChanged.AddListener(delegate
            {
                if (SoundManager.Instance != null)
                    SoundManager.Instance.Play(m_Type);
            });
        }
    }
}