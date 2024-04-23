using AssetKits.ParticleImage;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class FingerTrail : MonoBehaviour
    {
        [SerializeField] private ParticleImage m_ParticleImage;

        private Touch touch;
        private int touchCount;
        private bool isMagnifierToolSelected;

        public void OnToolSelect(Transform tool)
        {
            if (tool.name == "Magnifier")
                isMagnifierToolSelected = true;
            else
                isMagnifierToolSelected = false;
        }

        private void Update()
        {
            if (isMagnifierToolSelected)
                return;

            touchCount = Input.touchCount;
            if (touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved && !EventSystem.current.IsPointerOverGameObject())
                {
                    m_ParticleImage.enabled = true;
                    SetPosition(Input.mousePosition);
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Stationary)
                    m_ParticleImage.enabled = false;
            }
        }


        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}