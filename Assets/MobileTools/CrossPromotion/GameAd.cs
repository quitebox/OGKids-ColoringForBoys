using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BestHTTP;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MobileTools
{
    public class GameAd : MonoBehaviour
    {
        [Serializable]
        public struct Game
        {
            public string Name;
            public string Description;
            public string IconURL;
        }

        [Header("UI")]
        [SerializeField] private string m_URL = "https://drive.google.com/uc?export=download&id=1Yizl1KXima-FMxa6DBRqb59PAKpKwsbr";
        [SerializeField] private Transform cardsParentTransform;
        [SerializeField] private GameObject gameCardPrefab;
        private List<RawImage> _images = new List<RawImage>();

        Game[] Games;
        private List<HTTPRequest> activeRequests = new List<HTTPRequest>();

        private bool isComplete = false;

        [Header("Events")]
        public UnityEvent<bool> OnComplete;


#if !BESTHTTP_DISABLE_CACHING
        private bool allDownloadedFromLocalCache;
#endif

        public void Generate()
        {
            Debug.Log("Generating...");
            if (!isComplete)
                DownloadGamesInfo(m_URL);
        }

        private async Task DownloadGamesInfo(string url)
        {
            var request = new HTTPRequest(new Uri(url));

            Games = JsonHelper.GetArray<Game>(await request.GetAsStringAsync());

            DownloadImages();
        }

        private void SetAndStretchToParentSize(RectTransform _mRect, RectTransform _parent)
        {
            _mRect.localScale = Vector3.one;

            //_mRect.sizeDelta = _parent.rect.size;
            _mRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
            _mRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 0);
            _mRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
            _mRect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, 0);

            _mRect.anchorMin = new Vector2(0, 0);
            _mRect.anchorMax = new Vector2(1, 1);
            _mRect.pivot = new Vector2(0.5f, 0.5f);

            _mRect.transform.SetParent(_parent);
        }

        private void DownloadImages()
        {
            Debug.Log("Downloading images...");

            // Set these metadatas to its initial values
#if !BESTHTTP_DISABLE_CACHING
            allDownloadedFromLocalCache = true;
#endif

            if (_images.Count > 0)
                for (int i = 0; i < _images.Count; i++)
                    Destroy(_images[i].transform.parent.parent.gameObject);

            _images = new List<RawImage>();

            for (int i = 0; i < Games.Length; i++)
            {
                GameObject card = Instantiate(gameCardPrefab);
                card.transform.SetParent(cardsParentTransform);
                SetAndStretchToParentSize(card.GetComponent<RectTransform>(), cardsParentTransform.GetComponent<RectTransform>());
                card.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{Games[i].Name}";
                _images.Add(card.transform.GetChild(0).GetChild(0).GetComponent<RawImage>());
            }

            // Randomaly activate one game card
            for (int i = 0; i < _images.Count; i++)
                _images[i].transform.parent.parent.gameObject.SetActive(false);

            _images[UnityEngine.Random.Range(0, _images.Count)].transform.parent.parent.gameObject.SetActive(true);

            for (int i = 0; i < Games.Length; i++)
            {
                // Set a blank placeholder texture, overriding previously downloaded texture
                _images[i].texture = null;
                _images[i].color = Color.clear;


                // Construct the request
                var request = new HTTPRequest(new Uri(Games[i].IconURL), ImageDownloaded);

                // Set the Tag property, we can use it as a general storage bound to the request
                request.Tag = _images[i];

                // Send out the request
                request.Send();

            }

            isComplete = true;

            OnComplete?.Invoke(isComplete);
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

        #region Json Helpers
        private class JsonHelper
        {
            public static T[] GetArray<T>(string json)
            {
                string newJson = "{\"data\":" + json + "}";
                Wrapper<T> w = UnityEngine.JsonUtility.FromJson<Wrapper<T>>(newJson);
                return w.data;
            }

            [Serializable]
            class Wrapper<T>
            {
                public T[] data;
            }
        }
        #endregion
    }
}