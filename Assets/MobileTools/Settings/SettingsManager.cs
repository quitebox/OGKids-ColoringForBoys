using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.Events;

namespace MobileTools
{
    public class SettingsManager : MonoBehaviour
    {
        [SerializeField] private Image m_Background;
        [SerializeField] private Transform m_Popup;

        public Toggle musicToggle;
        public Toggle sfxToggle;
        public Toggle vibrationToggle;
        public Toggle notificationToggle;
        public AudioMixer musicAudioMixer;
        public AudioMixer sfxAudioMixer;

        [Space]
        [SerializeField] private string supportEmail = "support@theogkids.com";
        [SerializeField] private string privacyPolicyURL = "https://theogkids.com/privacy-policy/";
        [SerializeField] private string feedbackFormURL = "https://forms.gle/GXh4NM6TFfQxzuxe8";

        [Space]
        [Header("Events")]
        public UnityEvent<string> AppVersionEvent;
        public UnityEvent OnOpen;
        public UnityEvent OnClose;


        public void OnEnable()
        {
            // Add Listeners
            if (musicToggle != null)
                musicToggle.onValueChanged.AddListener((value) =>
                {
                    if (value)
                        musicAudioMixer.SetFloat("Volume", 0f);
                    else
                        musicAudioMixer.SetFloat("Volume", -80f);

                    // Save
                    if (GameSaves.Instance != null)
                    {
                        GameSaves.Instance.Value.MusicIsOn = value;
                        GameSaves.Instance.Save();
                    }
                });

            if (sfxToggle != null)
                sfxToggle.onValueChanged.AddListener((value) =>
                {
                    if (value)
                        sfxAudioMixer.SetFloat("Volume", 0f);
                    else
                        sfxAudioMixer.SetFloat("Volume", -80f);

                    // Save
                    if (GameSaves.Instance != null)
                    {
                        GameSaves.Instance.Value.SfxIsOn = value;
                        GameSaves.Instance.Save();
                    }
                });

            if (vibrationToggle != null)
                vibrationToggle.onValueChanged.AddListener((value) =>
                {
                    // TODO: Add game vibration code

                    // Save
                    if (GameSaves.Instance != null)
                    {
                        GameSaves.Instance.Value.VibrationIsOn = value;
                        GameSaves.Instance.Save();
                    }

                });
            if (notificationToggle != null)
                notificationToggle.onValueChanged.AddListener((value) =>
                {
                    // TODO: Add local game notification code

                    // Save
                    if (GameSaves.Instance != null)
                    {
                        GameSaves.Instance.Value.NotificationIsOn = value;
                        GameSaves.Instance.Save();
                    }
                });

            RefreshUI();

            OnOpen?.Invoke();
        }

        public void Close()
        {
            OnClose?.Invoke();
        }

        private void RefreshUI()
        {
            AppVersionEvent?.Invoke($"{Application.version}v");

            if (GameSaves.Instance == null)
                return;

            if (musicToggle != null)
                musicToggle.isOn = GameSaves.Instance.Value.MusicIsOn;
            if (sfxToggle != null)
                sfxToggle.isOn = GameSaves.Instance.Value.SfxIsOn;
            if (vibrationToggle != null)
                vibrationToggle.isOn = GameSaves.Instance.Value.VibrationIsOn;
            if (notificationToggle != null)
                notificationToggle.isOn = GameSaves.Instance.Value.NotificationIsOn;

            if (GameSaves.Instance.Value.MusicIsOn)
                musicAudioMixer.SetFloat("Volume", 0f);
            else
                musicAudioMixer.SetFloat("Volume", -80f);

            if (GameSaves.Instance.Value.SfxIsOn)
                sfxAudioMixer.SetFloat("Volume", 0f);
            else
                sfxAudioMixer.SetFloat("Volume", -80f);
        }

        public void HandlePrivacyPolicyButtonPressed()
        {
            Application.OpenURL("https://theogkids.com/privacy-policy/");
        }

        public void HandlePrivacySupportPressed()
        {
            Application.OpenURL($"mailto:{supportEmail}?subject={Application.productName} Support");
        }

        public void HandleFeedbackButtonPressed()
        {
            Application.OpenURL(feedbackFormURL);
        }
    }
}