using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class DeactivateAfterSomeTime : MonoBehaviour
    {
        [SerializeField] public float waitTime = 1f;

        [Space]
        [Header("Events")]
        [SerializeField] public UnityEvent OnWaitEnd;

        private void OnEnable()
        {
            StartCoroutine(WaitRoutine());
        }

        private IEnumerator WaitRoutine()
        {
            yield return new WaitForSeconds(waitTime);

            OnWaitEnd?.Invoke();
        }
    }
}