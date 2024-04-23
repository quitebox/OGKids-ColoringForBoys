using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class ResetButton : MonoBehaviour
    {
        private Camera _camera;

        private float _maxSize = 0;
        private Vector3 m_DefailtPosition = Vector3.zero;

        [Header("Events")]        
        public UnityEvent OnChange;
        public UnityEvent OnReset;

        private void Start()
        {
            Invoke("GetCamera", 0.3f);
        }

        private void GetCamera()
        {
            _camera = GetComponent<Camera>();
            _maxSize = _camera.orthographicSize;
            m_DefailtPosition = transform.localPosition;
        }

        public void Reset()
        {
            _camera.orthographicSize = _maxSize;
            _camera.transform.localPosition = m_DefailtPosition;            

            OnReset?.Invoke();
        }

        private void Update()
        {
            if (m_DefailtPosition.x != transform.localPosition.x || m_DefailtPosition.y != transform.localPosition.y)
                OnChange?.Invoke();
            
        }
    }
}