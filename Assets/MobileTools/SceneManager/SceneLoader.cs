using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace MobileTools
{
    public class SceneLoader : MonoBehaviour
    {
        public void LoadScene(int sceneBuildIndex)
        {
            if (SceneManager.Instance != null)
                SceneManager.Instance.LoadScene(sceneBuildIndex);
        }
    }
}
