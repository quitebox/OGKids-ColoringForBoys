using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class UISliderValueChanged : MonoBehaviour
    {
        Slider m_Slider;
        [SerializeField] SoundManager.SFX_TYPE m_Type;

        private void Start()
        {
            m_Slider = GetComponent<Slider>();
            m_Slider.onValueChanged.AddListener(delegate
            {
                if (SoundManager.Instance != null)
                    SoundManager.Instance.Play(m_Type, true);
            });
        }
    }
}