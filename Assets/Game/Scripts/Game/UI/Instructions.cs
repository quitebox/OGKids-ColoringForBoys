using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class Instructions : MonoBehaviour
    {
        [Header("UI Referances")]
        public GameObject StepA;
        public GameObject StepB;
        public GameObject StepC;
        public GameObject StepD;
        public GameObject StepE;
        public Button magnifierToolButton;
        public Button brushToolButton;
        public Button canvasResetButton;
        public Camera screenAndInputCamera;

        private float orthographicSize = 0f;

        [Space]
        [Header("Events")]
        public UnityEvent<bool> SetIntructionsUI;

        private void Start()
        {
            orthographicSize = screenAndInputCamera.orthographicSize;

            // setup buttons
            magnifierToolButton.onClick.AddListener(StepAComplete);

            // reset canvas
            canvasResetButton.onClick.AddListener(SetpEComplete);

            CheckInstructions();
        }

        // 1 Select Mangnifier 
        private void StepAComplete()
        {
            magnifierToolButton.onClick.RemoveListener(StepAComplete);

            StepA.gameObject.SetActive(false);
            StepB.gameObject.SetActive(true);
        }

        // 2 Zoom the canvas
        private void StepBComplete()
        {
            StepB.gameObject.SetActive(false);
            StepC.gameObject.SetActive(true);

            brushToolButton.onClick.AddListener(StepCComplete);
        }

        // 3 Select brush tool
        private void StepCComplete()
        {
            brushToolButton.onClick.RemoveListener(StepCComplete);

            StepC.gameObject.SetActive(false);
            StepD.gameObject.SetActive(true);
        }

        // 4 Draw on canvas
        private void StepDComplete()
        {
            StepD.gameObject.SetActive(false);
            StepE.gameObject.SetActive(true);
        }

        private void SetpEComplete()
        {
            StepE.gameObject.SetActive(false);

            SetShown();
        }

        private void CheckInstructions()
        {
            bool active = (PlayerPrefs.GetString("InstructionsUI", "NOT_SHOWN") == "NOT_SHOWN") ? true : false;
            SetIntructionsUI?.Invoke(active);

            if (!active)
                Destroy(gameObject);
        }

        public void SetShown()
        {
            PlayerPrefs.SetString("InstructionsUI", "SHOWN");
            CheckInstructions();
        }

        public void SetNotShown()
        {
            PlayerPrefs.SetString("InstructionsUI", "NOT_SHOWN");
            CheckInstructions();
        }

        private void Update()
        {
            // Check for step B
            if (StepB.activeSelf)
                if (screenAndInputCamera.orthographicSize != orthographicSize)
                    StepBComplete();

            // Check for step D
            if (StepD.activeSelf)
                if (Input.touchCount > 0)
                    if (Input.GetTouch(0).phase == TouchPhase.Moved)
                        StepDComplete();
        }
    }
}