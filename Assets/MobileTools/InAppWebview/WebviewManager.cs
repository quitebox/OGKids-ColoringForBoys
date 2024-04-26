//using MobileTools.Utilities;
//using UnityEngine;

//namespace MobileTools
//{
//    public class WebviewManager : Manager<WebviewManager>
//    {
//        [SerializeField] private GameObject m_UI;
//        [SerializeField] private RectTransform referenceRectTransform;
//        UniWebView m_WebView;
//        [SerializeField] private Rect frame;

//        public void LoadURL(string url)
//        {
//            if (m_WebView != null)
//                Close();

//            // Create a game object to hold UniWebView and add component.
//            var webViewGameObject = new GameObject("UniWebView");
//            m_WebView = webViewGameObject.AddComponent<UniWebView>();

//            Rect rect = NextFrameRect();
//            m_WebView.Frame = new Rect((int)rect.x, (int)rect.y, (int)rect.width, (int)rect.height);

//            m_WebView.SetHorizontalScrollBarEnabled(false);
//            m_WebView.SetVerticalScrollBarEnabled(false);

//            m_WebView.Load(url);
//            m_WebView.Show();            

//            m_UI.SetActive(true);
//        }

//        private void Update()
//        {
//            if (Input.GetKeyUp(KeyCode.Escape) && m_UI.activeSelf)
//                Close();
//        }

//        public void Close()
//        {
//            Destroy(m_WebView);
//            m_WebView = null;

//            m_UI.SetActive(false);
//        }

//        Rect NextFrameRect()
//        {
//            if (referenceRectTransform == null)
//            {
//                UniWebViewLogger.Instance.Info("Using Frame setting to determine web view frame.");
//                return frame;
//            }
//            else
//            {
//                UniWebViewLogger.Instance.Info("Using reference RectTransform to determine web view frame.");
//                var worldCorners = new Vector3[4];

//                referenceRectTransform.GetWorldCorners(worldCorners);

//                var bottomLeft = worldCorners[0];
//                var topLeft = worldCorners[1];
//                var topRight = worldCorners[2];
//                var bottomRight = worldCorners[3];

//                var canvas = referenceRectTransform.GetComponentInParent<Canvas>();
//                if (canvas == null)
//                {
//                    return frame;
//                }

//                switch (canvas.renderMode)
//                {
//                    case RenderMode.ScreenSpaceOverlay:
//                        break;
//                    case RenderMode.ScreenSpaceCamera:
//                    case RenderMode.WorldSpace:
//                        var camera = canvas.worldCamera;
//                        if (camera == null)
//                        {
//                            UniWebViewLogger.Instance.Critical(@"You need a render camera 
//                        or event camera to use RectTransform to determine correct 
//                        frame for UniWebView.");
//                            UniWebViewLogger.Instance.Info("No camera found. Fall back to ScreenSpaceOverlay mode.");
//                        }
//                        else
//                        {
//                            bottomLeft = camera.WorldToScreenPoint(bottomLeft);
//                            topLeft = camera.WorldToScreenPoint(topLeft);
//                            topRight = camera.WorldToScreenPoint(topRight);
//                            bottomRight = camera.WorldToScreenPoint(bottomRight);
//                        }
//                        break;
//                }

//                float widthFactor = (float)UniWebViewInterface.NativeScreenWidth() / (float)Screen.width;
//                float heightFactor = (float)UniWebViewInterface.NativeScreenHeight() / (float)Screen.height;

//                float x = topLeft.x * widthFactor;
//                float y = (Screen.height - topLeft.y) * heightFactor;
//                float width = (bottomRight.x - topLeft.x) * widthFactor;
//                float height = (topLeft.y - bottomRight.y) * heightFactor;
//                return new Rect(x, y, width, height);
//            }
//        }
//    }
//}
