using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class ParticleImageSoundHandler : MonoBehaviour
    {
        [SerializeField] private SoundManager.SFX_TYPE m_Type;

        public void OnEnable()
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.Play(m_Type);
        }
    }
}