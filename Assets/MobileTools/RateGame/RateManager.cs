using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MobileTools
{
    public class RateManager : MonoBehaviour
    {
        [Header("Platform IDs")]
        [SerializeField] private string iOSAppId;        

        [Space]
        [Header("UI References")]
        [SerializeField] private Image[] buttons;

        [Space]
        [Header("Values")]
        [SerializeField] [Range(1, 5)] private int ratingLimit = 4;
        [SerializeField] private Color selectColor = Color.white;
        [SerializeField] private Color defaultColor = Color.white;
        [SerializeField] private int rating = 0;

        [Space]
        [Header("Events")]
        public UnityEvent OnLowRatings;
        public UnityEvent OnHighRatings;    
        public UnityEvent OnCloseButtonClick;

        public void OnStarSelect(Image button)
        {
            rating = 0;
            for (int i = 0; i < buttons.Length; i++)
            {
                rating++;
                buttons[i].color = selectColor;

                if (buttons[i] == button)
                    break;
            }

            if (rating >= ratingLimit)
                Invoke("RatedHigh", 0.6f);
            else
                Invoke("RatedLow", 0.6f);

            Invoke("Close", 0.3f);
        }

        private void RatedHigh()
        {
            if (Application.platform == RuntimePlatform.Android)
                Application.OpenURL($"https://play.google.com/store/apps/details?id={Application.identifier}");
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                Application.OpenURL($"https://apps.apple.com/app/id{iOSAppId}");

            OnHighRatings?.Invoke();
        }

        private void RatedLow()
        {
            OnLowRatings.Invoke();
        }

        private void OnEnable()
        {
            // Reset colors
            for (int i = 0; i < buttons.Length; i++)
                buttons[i].color = defaultColor;
        }

        public void Close()
        {
            OnCloseButtonClick?.Invoke();
        }
    }
}