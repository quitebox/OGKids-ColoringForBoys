using UnityEngine;
using UnityEngine.UI;

namespace MobileTools
{
    public class LanguageDropdownNGUI : MonoBehaviour
    {
        Dropdown m_Dropdown;
        public bool restartScene = false;

        private void Start()
        {
            m_Dropdown = GetComponent<Dropdown>();
            m_Dropdown.value = LocallizationManager.Instance.GetCurrentLanguateIndex;
            m_Dropdown.onValueChanged.AddListener(HandleValueChanged);
        }

        private void HandleValueChanged(int index)
        {
            LocallizationManager.Instance.SetCurrentLanguage(m_Dropdown.options[index].text);

            //SceneManager.Instance.LoadScene(0);
        }
    }
}