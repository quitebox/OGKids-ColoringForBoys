using UnityEngine;

namespace Game
{
    public class SoundTypePlayer : MonoBehaviour
    {
        [SerializeField] SoundManager.SFX_TYPE m_Type;
        [SerializeField] bool m_PlayOnEnable = false;
        [SerializeField] bool m_PlayOnDisable = false;

        private void OnEnable()
        {
            if (m_PlayOnEnable)
                Play();
        }

        private void OnDisable()
        {
            if (m_PlayOnDisable)
                Play();
        }

        public void Play()
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.Play(m_Type);
        }
    }
}