using System.Collections;
using System.Collections.Generic;
using MobileTools.Utilities;
using UnityEngine;

namespace Game
{
    public class SoundManager : Manager<SoundManager>
    {
        public enum SFX_TYPE
        {
            None, Button, CancelButton, Toggle, Slider, DropDown, ColorWheel, PreviewImage, ConfettiBlast, ToolSelect, ClearCanvas, DrawOnCanvas, PaintBucket
        }

        public enum MUSIC_TYPE
        {
            None, BGM_Menu, BGM_PageSelection, BGM_Game
        }

        [System.Serializable]
        public class Audio
        {
            [HideInInspector] public int index;
            [HideInInspector] public string name;
            public AudioSource souce;
        }

        [System.Serializable]
        public class SFX
        {
            public Audio audio;
            public SFX_TYPE type;
            [Range(0f, 1f)] public float volume = 1f;
        }

        [System.Serializable]
        public class Music
        {
            public Audio audio;
            public MUSIC_TYPE type;
            [Range(0f, 1f)] public float volume = 1f;
        }

        [SerializeField] private List<Music> m_Musics = new List<Music>();
        [SerializeField] private List<SFX> m_SFXs = new List<SFX>();

        private AudioSource playedMusicAudioSource;

        public AudioSource ActiveAudioSource { get { return playedMusicAudioSource; } }

        public void Play(SFX_TYPE type)
        {
            foreach (var sfx in m_SFXs)
                if (type == sfx.type)
                {
                    sfx.audio.souce.volume = sfx.volume;
                    sfx.audio.souce.Play();
                }
        }

        public void Play(SFX_TYPE type, bool waitForFinishAudio)
        {
            foreach (var sfx in m_SFXs)
                if (type == sfx.type)
                {
                    if (!sfx.audio.souce.isPlaying)
                    {
                        sfx.audio.souce.volume = sfx.volume;
                        sfx.audio.souce.Play();
                    }
                }
        }

        public void Play(MUSIC_TYPE type)
        {
            foreach (var music in m_Musics)
                if (type == music.type)
                {
                    music.audio.souce.volume = music.volume;
                    music.audio.souce.Play();
                    playedMusicAudioSource = music.audio.souce;
                }
        }

        public void Play(MUSIC_TYPE type, bool loop)
        {
            foreach (var music in m_Musics)
                if (type == music.type)
                {
                    music.audio.souce.volume = music.volume;
                    music.audio.souce.Play();
                    music.audio.souce.loop = loop;
                    playedMusicAudioSource = music.audio.souce;
                }
        }

        public void Play(MUSIC_TYPE type, bool loop, bool smoothTransition)
        {
            foreach (var music in m_Musics)
                if (type == music.type)
                {
                    music.audio.souce.loop = loop;

                    //if (playedMusicAudioSource == null)
                    //    playedMusicAudioSource = music.audio.souce;

                    if (smoothTransition && playedMusicAudioSource != music.audio.souce)
                        StartCoroutine(SmoothPlayRoutine(music));
                }
        }

        private IEnumerator SmoothPlayRoutine(Music music)
        {
            float FadeTime = 0.3f;

            //music.audio.souce.Stop();
            music.audio.souce.volume = 0;
            music.audio.souce.Play();

            while (playedMusicAudioSource.volume > 0)
            {
                // Fade Out
                playedMusicAudioSource.volume -= Mathf.Lerp(playedMusicAudioSource.volume, 0f, Time.deltaTime / FadeTime);

                // Fade In
                music.audio.souce.volume = Mathf.Lerp(music.audio.souce.volume, music.volume, Time.deltaTime / FadeTime);

                if (music.audio.souce.volume >= music.volume)
                    yield break;

                yield return null;
            }

            //playedMusicAudioSource.Stop();
            playedMusicAudioSource.volume = 0;

            music.audio.souce.volume = music.volume;
            playedMusicAudioSource = music.audio.souce;            
        }

        //private void Update()
        //{
        //    if (Input.GetKeyDown(KeyCode.Space))
        //        Play((SFX_TYPE)Random.Range(1, 12));

        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //        Play((MUSIC_TYPE)(1), true, true);
        //    else if (Input.GetKeyDown(KeyCode.Alpha2))
        //        Play((MUSIC_TYPE)(2), true, true);
        //    else if (Input.GetKeyDown(KeyCode.Alpha3))
        //        Play((MUSIC_TYPE)(3), true, true);
        //}
    }
}