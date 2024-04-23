using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MobileTools
{
    public class SocialShare : MonoBehaviour
    {
        [SerializeField] private string m_GameFeatures;
        [SerializeField] private string m_IOSAppStoreID;

        private string m_ShareMessage = string.Empty;
        private string m_FilePath = null;

        private void Start()
        {
            string gameLink = string.Empty;

            if (Application.platform == RuntimePlatform.Android)
                gameLink = $"https://play.google.com/store/apps/details?id={Application.identifier}";
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                gameLink = $"https://apps.apple.com/app/id{m_IOSAppStoreID}";

            m_ShareMessage = $"Hey there! I just played {Application.productName} and it's awesome! It's {m_GameFeatures}. I couldn't put it down. You should give it a try! Download it now {gameLink}";
        }

        public void SetFilePath(string filepath)
        {
            m_FilePath = filepath;
        }

        public void Share()
        {
            if (m_FilePath == string.Empty)
                return;

            new NativeShare().AddFile(m_FilePath)
            .SetText($"{m_ShareMessage}")
            .SetCallback((result, shareTarget) => Debug.Log("Share result: " + result + ", selected app: " + shareTarget))
            .Share();
        }
    }
}
