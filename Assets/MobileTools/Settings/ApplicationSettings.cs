using UnityEngine;

namespace MobileTools
{
    public class ApplicationSettings : MonoBehaviour
    {
        private void Awake()
        {
            Application.targetFrameRate = 30;

#if UNITY_EDITOR
            Debug.unityLogger.logEnabled = true;
#else
  Debug.unityLogger.logEnabled = false;
#endif
        }
    }
}
