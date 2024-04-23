using UnityEngine;
using UnityEngine.UI;

namespace MobileTools
{
    public class IAPRestoreButtonActivation : MonoBehaviour
    {
        [SerializeField] private Button m_RestoreButton;

        public void OnEnable()
        {
            if (Application.platform == RuntimePlatform.Android)
                m_RestoreButton.interactable =  false;
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
                m_RestoreButton.interactable = true;
        }
    }
}