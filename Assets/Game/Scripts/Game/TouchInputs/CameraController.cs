using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class CameraController : MonoBehaviour
    {
        private Camera _camera; // Reference to the orthographic camera

        [SerializeField] private float _minSize = 1f; // Minimum orthographic size
        [SerializeField] private float _maxSize = 1000f; // Maximum orthographic size
        [SerializeField] private float _zoomSpeed = 0.001f; // Zoom speed

        private Vector3 m_DefailtPosition = Vector3.zero;
        private int m_TouchCount = 0;

        public CanvasGroup footerUICanvasGroup;

        [Header("Events")]
        public UnityEvent<bool> OnOneFingersDetection;
        public UnityEvent<bool> OnTwoFingersDetection;
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

            //StartCoroutine(ShowFooterUIRoutine());

            OnReset?.Invoke();
        }

        private void Update()
        {
            m_TouchCount = Input.touchCount;

            OnOneFingersDetection?.Invoke(!(m_TouchCount == 1));
            OnTwoFingersDetection?.Invoke(!(m_TouchCount == 2));

            if (m_DefailtPosition.x != transform.localPosition.x || m_DefailtPosition.y != transform.localPosition.y)
            {
                //StartCoroutine(HideFooterUIRoutine());

                OnChange?.Invoke();
            }

            if (m_TouchCount == 2) // Check if there are two fingers on the screen
            {                
                if (Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    //StartCoroutine(HideFooterUIRoutine());

                    OnChange?.Invoke();

                    Touch touch0 = Input.GetTouch(0);
                    Touch touch1 = Input.GetTouch(1);

                    Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
                    Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

                    float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
                    float touchDeltaMag = (touch0.position - touch1.position).magnitude;
                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

                    float cameraSize = _camera.orthographicSize;
                    cameraSize += deltaMagnitudeDiff * _zoomSpeed;

                    // Clamp camera size between min and max bounds
                    cameraSize = Mathf.Clamp(cameraSize, _minSize, _maxSize);

                    // Set the new orthographic size of the camera
                    _camera.orthographicSize = cameraSize;
                }
            }
        }


        private IEnumerator HideFooterUIRoutine() {

            footerUICanvasGroup.interactable = false;

            float fadeTime = 0.3f;

            while (footerUICanvasGroup.alpha > 0)
            {
                footerUICanvasGroup.alpha -= 0.1f * Time.deltaTime / fadeTime;

                if (footerUICanvasGroup.alpha <= 0)
                    break;

                yield return null;
            }            
        }

        private IEnumerator ShowFooterUIRoutine()
        {
            footerUICanvasGroup.interactable = true;

            float fadeTime = 0.1f;

            while (footerUICanvasGroup.alpha < 1)
            {
                footerUICanvasGroup.alpha += 0.1f * Time.deltaTime / fadeTime;

                if (footerUICanvasGroup.alpha >= 1)
                    break;

                yield return null;
            }            
        }
    }
}