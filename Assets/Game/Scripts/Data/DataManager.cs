using System.Collections;
using System.Collections.Generic;
using Game;
using Ricimi;
using SimpleJSON;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.UI;
using BestHTTP;
using System;
using MobileTools;
using System.IO;

public class DataManager : MonoBehaviour
{
    private enum Environment
    {
        dev, prod
    }

    [SerializeField] private Environment m_Env;

    [SerializeField] private string m_BaseURL = "https://theogkids.com/cms/games/com.ogkids.coloringforboys/";
    [SerializeField] private string m_DataJsonURL = "data.php";

    [Header("Data")]
    private string filePath;

    [Header("Category")]
    [SerializeField] private GameObject m_CategoryButtonPrefab;
    [SerializeField] private Transform m_CategoryButtonsParentT;
    private List<GameObject> m_SpawnedCategoriesList = new List<GameObject>();
    private List<GameObject> m_SpawnedSubCategoriesList = new List<GameObject>();

    [Header("Sub Category")]
    [SerializeField] private GameObject m_SubCategoryPrefab;

    [Header("Page Button")]
    [SerializeField] private GameObject m_PageButtonPrefab;


    [SerializeField] List<RawImage> pageOutlineRawImageList = new List<RawImage>();
    [SerializeField] List<RawImage> pageRegionRawImageList = new List<RawImage>();
    [SerializeField] List<RawImage> categoryRawImageList = new List<RawImage>();

    [SerializeField] List<string> pageOutlineUrlList = new List<string>();
    [SerializeField] List<string> pageRegionUrlList = new List<string>();
    [SerializeField] List<string> categoryUrlList = new List<string>();

    [Header("Events")]
    public UnityEvent OnStart;
    public UnityEvent OnComplete;
    public UnityEvent OnGameLoad;
    public UnityEvent OnGameUnload;

    private void SaveData(string path, string data, UnityAction OnComplete)
    {
        File.WriteAllText(path, data);
        Debug.Log("Data saved to: " + filePath);
        OnComplete?.Invoke();
    }

    private void LoadData(string path, UnityAction<string> OnComplete)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(filePath);
            OnComplete?.Invoke(json);
        }
        else
        {
            Debug.LogWarning("No file found at: " + filePath);
        }
    }

    private void Awake()
    {
        OnStart?.Invoke();

        filePath = Path.Combine(Application.persistentDataPath, "gamedata.json");

        StartCoroutine(ConnectivityChecker.CheckInternet((bool result) =>
        {
            if (result)
            {
                m_DataJsonURL = $"{m_BaseURL}{m_DataJsonURL}?env={m_Env.ToString().ToLower()}";
                StartCoroutine(GetJsonData(m_DataJsonURL, (string jsonData) =>
                {
                    SaveData(filePath, jsonData, () =>
                    {
                        Init(filePath);
                    });
                }));
            }
            else
            {
                Init(filePath);
            }
        }));
    }

    private void Init(string path)
    {
        LoadData(path, (string data) =>
        {
            var parsedData = JSON.Parse(data);

            // Create Categories
            ClearCategories();

            foreach (string item in parsedData.Keys)
            {
                CreateCategories(item, data);
            }

            // Add Scritable objects for offline data processing then call download images
            StartCoroutine(DownloadImages(categoryRawImageList, categoryUrlList));

            OnComplete?.Invoke();
        });
    }

    private IEnumerator GetJsonData(string url, UnityAction<string> OnComplete)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + webRequest.error);
            }
            else
            {
                // Get the JSON text
                string jsonData = webRequest.downloadHandler.text;
                OnComplete?.Invoke(jsonData);
            }
        }
    }

    private void ClearCategories()
    {
        for (int i = 0; i < m_SpawnedCategoriesList.Count; i++)
            Destroy(m_SpawnedCategoriesList[i]);

        m_SpawnedCategoriesList.Clear();
    }

    private void ClearSubCategories()
    {
        for (int i = 0; i < m_SpawnedSubCategoriesList.Count; i++)
            Destroy(m_SpawnedSubCategoriesList[i]);

        pageOutlineRawImageList.Clear();
        pageRegionRawImageList.Clear();        
        m_SpawnedSubCategoriesList.Clear();
    }

    private void CreateCategories(string categoryName, string jsonData)
    {
        var parsedData = JSON.Parse(jsonData);
        GameObject categoryButton = Instantiate(m_CategoryButtonPrefab, m_CategoryButtonsParentT);
        categoryButton.name = categoryName;
        string imageUrl = $"{m_BaseURL}{m_Env}/{categoryName}/{parsedData[categoryName][1].Value}";
        RawImage rawImage = categoryButton.transform.GetChild(0).GetComponent<RawImage>();
        TextMeshProUGUI nameText = categoryButton.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        nameText.text = $"{categoryName}";
        categoryRawImageList.Add(rawImage);
        categoryUrlList.Add(imageUrl);

        categoryButton.AddComponent<PopupOpener>();
        PopupOpener popupOpener = categoryButton.GetComponent<PopupOpener>();
        categoryButton.GetComponent<Button>().onClick.AddListener(delegate
        {
            ClearSubCategories();

            GameObject newSubCategory = Instantiate(m_SubCategoryPrefab, FindAnyObjectByType<Canvas>().transform);
            newSubCategory.name = $"sub-{categoryName}";

            Transform pageButtonParentT = newSubCategory.transform.GetChild(0).GetChild(1).GetChild(0).GetChild(0).GetChild(0);
            for (int i = 0; i < parsedData[categoryName][0].Count; i++)
            {
                //Debug.Log(parsedData[categoryName][0][i]);
                GameObject newPageButton = Instantiate(m_PageButtonPrefab, pageButtonParentT);
                newPageButton.name = $"{categoryName}-{i}";

                ColorPageButton colorPageButton = newPageButton.GetComponent<ColorPageButton>();
                colorPageButton.m_PageConfig.name = newPageButton.name;
                colorPageButton.outlineURL = $"{m_BaseURL}{m_Env}/{categoryName}/images/{i + 1}/{parsedData[categoryName][0][i][0].Value}";
                colorPageButton.regionURL = $"{m_BaseURL}{m_Env}/{categoryName}/images/{i + 1}/{parsedData[categoryName][0][i][1].Value}";

                newPageButton.gameObject.SetActive(true);

                //pageOutlineRawImageList.Add(newPageButton.transform.GetChild(0).GetChild(0).GetComponent<RawImage>());
                //pageRegionRawImageList.Add(newPageButton.transform.GetChild(0).GetComponent<RawImage>());

                //string outlineImageUrl = $"{m_BaseURL}{m_Env}/{categoryName}/images/{i + 1}/{parsedData[categoryName][0][i][0].Value}";
                //pageOutlineUrlList.Add(outlineImageUrl);

                //string regionImageUrl = $"{m_BaseURL}{m_Env}/{categoryName}/images/{i + 1}/{parsedData[categoryName][0][i][1].Value}";
                //pageRegionUrlList.Add(regionImageUrl);
            }

            popupOpener.popupPrefab = newSubCategory;
            popupOpener.OpenPopup();
            m_SpawnedSubCategoriesList.Add(newSubCategory);

            //StartCoroutine(DownloadImages(pageOutlineRawImageList, pageOutlineUrlList));
            //StartCoroutine(DownloadImages(pageRegionRawImageList, pageRegionUrlList));
        });
    }

    private List<HTTPRequest> activeRequests = new List<HTTPRequest>();

#if !BESTHTTP_DISABLE_CACHING
    private bool allDownloadedFromLocalCache;
#endif    

    private IEnumerator DownloadImages(List<RawImage> _images, List<string> pageUrls)
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

    public void LoadGame()
    {
        Popup[] popups = FindObjectsOfType<Popup>();

        for (int i = 0; i < popups.Length; i++)
            popups[i].Close();

        OnGameLoad?.Invoke();
    }

    public void UnloadGame()
    {
        OnGameUnload?.Invoke();
    }
}
