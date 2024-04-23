using System.Collections;
using System.Collections.Generic;
using MobileTools;
using Ricimi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class CategoryLocker : MonoBehaviour, IPointerClickHandler
    {
        public GameObject lockUI;

        [Space]
        [Header("Events")]
        public UnityEvent OnPointerDownWhenLocked;

        private GameObject spawnedLockUI;

        private void OnEnable()
        {
            if (GameSaves.Instance != null)
            {
                GameSaves.Instance.OnSave.AddListener(delegate
                {
                    CheckAppLock();
                });

                CheckAppLock();
            }
        }

        private void OnDisable()
        {
            if (GameSaves.Instance != null)
                GameSaves.Instance.OnSave.RemoveAllListeners();

            if (spawnedLockUI != null)
                Destroy(spawnedLockUI);
        }

        private void CheckAppLock()
        {
            if (!GameSaves.Instance.Value.HasUnlockedFullApp)
            {
                spawnedLockUI = Instantiate(lockUI, transform);
                GetComponent<AnimatedButton>().interactable = false;
            }
            else
            {
                if (spawnedLockUI != null)
                {
                    GetComponent<AnimatedButton>().interactable = true;
                    Destroy(spawnedLockUI);
                }
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (spawnedLockUI != null)
                OnPointerDownWhenLocked?.Invoke();
        }
    }
}