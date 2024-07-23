using UnityEngine;

namespace Game
{
    public class BGMPlayer : MonoBehaviour
    {
        public SoundManager.MUSIC_TYPE musicType;

        private void Start()
        {
            if (SoundManager.Instance != null)
            {
                if (SoundManager.Instance.ActiveAudioSource == null)
                    SoundManager.Instance.Play(musicType, true);
                else
                    SoundManager.Instance.Play(musicType, true, true);
            }
        }

        public void Restart()
        {
            Start();
        }
    }
}