using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

namespace MobileTools
{
    public class ConnectivityChecker : MonoBehaviour
    {
        [SerializeField] private float m_InternetCheckInterval = 3;

        [Space]
        [Header("Events")]
        public UnityEvent OnInternetReachable;
        public UnityEvent OnInternetNotReachable;


        private void Start()
        {
            InvokeRepeating("CheckInternet", 0, m_InternetCheckInterval);
        }

        private void CheckInternet()
        {
            Debug.Log($"[{name}]: Checking internet...");
            StopAllCoroutines();
            StartCoroutine(CheckInternetRoutine());
        }

        private IEnumerator CheckInternetRoutine()
        {
            UnityWebRequest request = new UnityWebRequest("http://google.com");
            yield return request.SendWebRequest();
            if (request.error != null)
            {
                Debug.Log("Error");
                OnInternetNotReachable?.Invoke();
            }
            else
            {
                Debug.Log("Success");
                OnInternetReachable?.Invoke();
            }
        }

        private void OnDisable()
        {
            CancelInvoke();
        }
    }
}