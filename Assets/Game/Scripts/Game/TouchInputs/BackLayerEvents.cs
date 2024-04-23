using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class BackLayerEvents : MonoBehaviour
    {
        public Camera cam;

        [Header("Events")]
        public UnityEvent<Vector3> OnTouchPhaseMoved;
        public UnityEvent<Vector3> OnTouchPhaseEnded;

        private void Update()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log($"{hit.transform.name}");
                //if (hit.transform.name == "BackLayer")
                //    if (Input.GetMouseButton(0))
                //        OnTouchPhaseMoved?.Invoke(Input.mousePosition);                                    

                //if (Input.GetMouseButtonUp(0))
                //    OnTouchPhaseEnded?.Invoke(Input.mousePosition);
            }
        }
    }
}
