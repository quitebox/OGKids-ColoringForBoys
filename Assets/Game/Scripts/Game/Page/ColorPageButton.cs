using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using BestHTTP;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using Ricimi;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class ColorPageButton : MonoBehaviour
    {
        [HideInInspector] public string outlineURL;
        [HideInInspector] public string regionURL;
        [SerializeField] public PageConfig m_PageConfig;

        [Header("References")]
        [SerializeField] private RawImage m_RegionImage;
        [SerializeField] private RawImage m_OutlineImage;
        [SerializeField] private GameObject m_SpinnerObject;

        private Texture2D m_Texture = null;

        private void OnEnable()
        {
            //if (m_RegionImage == null)
            //{
            //    m_RegionImage = transform.GetChild(0).GetComponent<RawImage>();
            //    m_RegionImage.raycastTarget = false;
            //}

            //if (m_OutlineImage == null)
            //{
            //    m_OutlineImage = transform.GetChild(0).GetChild(0).GetComponent<RawImage>();
            //    m_OutlineImage.raycastTarget = false;
            //}

            StartCoroutine(UpdateTextures());
            //UpdateUI();
        }

        private void _OnDisable()
        {
            if (m_Texture != null)
            {
                Destroy(m_RegionImage.texture);
                Destroy(m_Texture);
            }
        }

        public void UpdateUI()
        {
            if (m_PageConfig == null)
                return;

            //if (m_PageConfig is ColoringPageConfig)
            //    m_OutlineImage.texture = (m_PageConfig as ColoringPageConfig).Icon.texture;


            if (File.Exists(m_PageConfig.IconSavePath) && m_PageConfig.name != null)
            {
                m_Texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
                if (m_Texture.LoadImage(File.ReadAllBytes(m_PageConfig.IconSavePath)))
                {
                    m_Texture.Apply(false, true);
                    m_RegionImage.texture = m_Texture;
                    //m_RegionImage.texture = Sprite.Create(m_Texture, new Rect(0, 0, m_Texture.width, m_Texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }

        public void SelectPage()
        {
            FindAnyObjectByType<DataManager>().LoadGame();

            PageData.pageName = name;
            PageData.region = (Texture2D)transform.GetChild(0).GetComponent<RawImage>().texture;
            PageData.outline = (Texture2D)transform.GetChild(0).GetChild(0).GetComponent<RawImage>().texture;

            AppData.SelectedPageConfig = m_PageConfig;

            GetComponent<SceneTransition>().PerformTransition();
        }




        #region Download textures from server
        private List<HTTPRequest> activeRequests = new List<HTTPRequest>();

#if !BESTHTTP_DISABLE_CACHING
        private bool allDownloadedFromLocalCache;
#endif 

        private IEnumerator UpdateTextures()
        {
            List<RawImage> rawImages = new List<RawImage>();
            rawImages.Add(m_RegionImage);
            rawImages.Add(m_OutlineImage);

            List<string> pageUrls = new List<string>();
            pageUrls.Add(regionURL);
            pageUrls.Add(outlineURL);

            StartCoroutine(DownloadImages(rawImages, pageUrls, () =>
            {
                Destroy(m_SpinnerObject);
            }));

            yield return new WaitForEndOfFrame();
        }

        private IEnumerator DownloadImages(List<RawImage> _images, List<string> pageUrls, UnityAction OnComplete)
        {
            Debug.Log("Downloading images...");

            // Set these metadatas to its initial values
#if !BESTHTTP_DISABLE_CACHING
            allDownloadedFromLocalCache = true;
#endif

            for (int i = 0; i < _images.Count; i++)
            {
                var request = new HTTPRequest(new Uri(pageUrls[i]), ImageDownloaded);
                request.Tag = _images[i];
                yield return request.Send();
            }

            OnComplete?.Invoke();
        }

        void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
        {
            switch (req.State)
            {
                case HTTPRequestStates.Finished:
                    if (resp.IsSuccess)
                    {
                        RawImage rawImage = req.Tag as RawImage;
                        rawImage.texture = resp.DataAsTexture2D;
                        rawImage.color = Color.white;

#if !BESTHTTP_DISABLE_CACHING
                        // Update the cache-info variable
                        allDownloadedFromLocalCache = allDownloadedFromLocalCache && resp.IsFromCache;
#endif

                    }
                    else
                    {
                        Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}",
                                                        resp.StatusCode,
                                                        resp.Message,
                                                        resp.DataAsText));
                    }
                    break;

                // The request finished with an unexpected error. The request's Exception property may contain more info about the error.
                case HTTPRequestStates.Error:
                    Debug.LogError("Request Finished with Error! " + (req.Exception != null ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
                    break;

                // The request aborted, initiated by the user.
                case HTTPRequestStates.Aborted:
                    Debug.LogWarning("Request Aborted!");
                    break;

                // Connecting to the server is timed out.
                case HTTPRequestStates.ConnectionTimedOut:
                    Debug.LogError("Connection Timed Out!");
                    break;

                // The request didn't finished in the given time.
                case HTTPRequestStates.TimedOut:
                    Debug.LogError("Processing the request Timed Out!");
                    break;
            }

            activeRequests.Remove(req);
            if (activeRequests.Count == 0)
            {
#if !BESTHTTP_DISABLE_CACHING
                if (allDownloadedFromLocalCache)
                    Debug.Log("All images loaded from local cache!");
                else
#endif
                    Debug.Log(string.Empty);
            }
        }


        #endregion


    }
}