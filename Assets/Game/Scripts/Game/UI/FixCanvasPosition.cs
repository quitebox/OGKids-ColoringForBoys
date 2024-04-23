using System.Collections;
using System.Collections.Generic;
using PaintCraft.Controllers;
using UnityEngine;

namespace Game
{
    public class FixCanvasPosition : MonoBehaviour
    {
        public Camera cam;
        public Canvas canvas;
        public GameObject worldObject;
        public RectTransform uiObject;
        public float percent = 0.1f;

        private Vector3 pos;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2);

            worldObject = FindObjectOfType<BackLayerController>().gameObject;
        }

        [ContextMenu("Test")]
        public void Test()
        {
            //uiObject.transform.localPosition = worldToUISpace(canvas, worldObject.transform.localPosition);
            //uiObject.transform.localPosition = cam.WorldToScreenPoint(worldObject.transform.localPosition);
            pos = cam.ScreenToWorldPoint(uiObject.transform.localPosition);
            pos.x = worldObject.transform.localPosition.x;
            pos.y = pos.y - (Screen.height * percent);
            pos.z = worldObject.transform.localPosition.z;

            worldObject.transform.localPosition = pos;
        }

        public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
        {
            //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
            Vector3 screenPos = cam.WorldToScreenPoint(worldPos);
            Vector2 movePos;

            //Convert the screenpoint to ui rectangle local point
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
            //Convert the local point to world point
            return parentCanvas.transform.TransformPoint(movePos);
        }
    }
}