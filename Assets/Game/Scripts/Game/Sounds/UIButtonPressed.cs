using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UIButtonPressed : MonoBehaviour
    {
        Button m_Button;
        [SerializeField] SoundManager.SFX_TYPE m_Type;

        private void Start()
        {
            m_Button = GetComponent<Button>();
            m_Button.onClick.AddListener(delegate
            {
                if (SoundManager.Instance != null)
                    SoundManager.Instance.Play(m_Type);
            });
        }
    }
}