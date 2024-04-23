using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MobileTools.Utilities;
using UnityEngine.Events;

namespace MobileTools
{
    public class SceneManager : Manager<SceneManager>
    {
        [SerializeField] private GameObject m_UI;
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private Slider m_Slider;
        [SerializeField] private TextMeshProUGUI m_Text;
        [SerializeField] private float m_Progress;
        [SerializeField] private bool m_AsyncLoad = false;
        [SerializeField] private int[] sceneIndexRequiresProgressbar;

        [Space]
        [Header("Event")]
        public UnityEvent OnStart;
        public UnityEvent OnEnd;

        public void LoadScene(int sceneBuildIndex)
        {
            m_Slider.value = 0;
            m_Text.text = "0%";

            // Enable loading UI
            m_UI.gameObject.SetActive(true);

            OnStart?.Invoke();

            for (int i = 0; i < sceneIndexRequiresProgressbar.Length; i++)
            {
                if (sceneIndexRequiresProgressbar[i] == sceneBuildIndex)
                    m_AsyncLoad = true;
                else
                    m_AsyncLoad = false;
            }

            // Fade in
            m_CanvasGroup.DOFade(1, 0.6f).OnComplete(delegate
            {
                StartCoroutine(LoadSceneRoutine(sceneBuildIndex));
            });           
        }

        private IEnumerator LoadSceneRoutine(int index)
        {
            if (m_AsyncLoad)
            {
                m_Progress = 0f;

                AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(index);

                asyncLoad.allowSceneActivation = false;

                while (m_Progress <= 1f)
                {
                    m_Slider.value = m_Progress;

                    m_Text.text = Mathf.Round(m_Progress * 100f) + "%";

                    m_Progress += .01f;

                    yield return new WaitForSeconds(.01f);
                }

                while (!asyncLoad.isDone && m_Progress >= 1f)
                {
                    asyncLoad.allowSceneActivation = true; //here the scene is definitely loaded.            

                    // Fade out
                    m_CanvasGroup.DOFade(0, 0.6f).OnComplete(delegate
                    {
                        m_UI.gameObject.SetActive(false);
                    });

                    yield return null;

                }
            }
            else
                UnityEngine.SceneManagement.SceneManager.LoadScene(index);

            OnEnd?.Invoke();
        }

    }
}
