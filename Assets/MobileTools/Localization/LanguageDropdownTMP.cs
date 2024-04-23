using UnityEngine;
using TMPro;

namespace MobileTools
{
    public class LanguageDropdownTMP : MonoBehaviour
    {
        TMP_Dropdown m_TMP_Dropdown;

        private void Start()
        {
            m_TMP_Dropdown = GetComponent<TMP_Dropdown>();
            m_TMP_Dropdown.value = LocallizationManager.Instance.GetCurrentLanguateIndex;
            m_TMP_Dropdown.onValueChanged.AddListener(HandleValueChanged);
        }

        private void HandleValueChanged(int index)
        {
            LocallizationManager.Instance.SetCurrentLanguage(m_TMP_Dropdown.options[index].text);
        }
    }
}