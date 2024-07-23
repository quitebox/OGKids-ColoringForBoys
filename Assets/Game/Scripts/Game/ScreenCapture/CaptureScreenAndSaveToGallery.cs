using System.Collections;
using System.IO;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using PaintCraft.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class CaptureScreenAndSaveToGallery : MonoBehaviour
    {
        private MeshRenderer outlineMeshR;

        private Texture2D outlineTex;
        private Texture2D regionTex;

        //[SerializeField] private Texture2D watermarkTex;
        [SerializeField] private CanvasController canvasController;
        [SerializeField] private RawImage outputImage;


        private string capturedImageFilePath = string.Empty;
        private string imageName = "temp.png";

        [Space]
        [Header("Events")]
        public UnityEvent OnStart;
        public UnityEvent<string> OnComplete;

        private void Start()
        {
            capturedImageFilePath = $"{new DirectoryInfo(Application.dataPath).Parent}/{imageName}";

            if (!Application.isEditor)
                capturedImageFilePath = $"{Application.persistentDataPath}/{imageName}";

        }

        private IEnumerator SaveRoutine()
        {
            OnStart?.Invoke();

            yield return new WaitForSeconds(0.3f);
            yield return new WaitForEndOfFrame();

            outlineMeshR = GameObject.Find("Outline").GetComponent<MeshRenderer>();
            //outlineTex = CopyTexture((Texture2D)outlineMeshR.material.mainTexture);
            outlineTex = (Texture2D)outlineMeshR.material.mainTexture;
            outlineTex = Resize(outlineTex, 512, 512);

            regionTex = new Texture2D(outlineTex.width, outlineTex.height);
            regionTex.LoadImage(File.ReadAllBytes(Application.persistentDataPath + "/Saves/" + canvasController.PageConfig.UniqueId + ".jpg"));
            regionTex = Resize(regionTex, 512, 512);

            Texture2D tex = MergeTextures(regionTex, outlineTex);
            //tex = MergeTextures(tex, watermarkTex);
            outputImage.texture = tex;

            string filePath = Path.Combine(Application.temporaryCachePath, "shared img.png");
            File.WriteAllBytes(filePath, tex.EncodeToPNG());

            // Save the screenshot to Gallery/Photos
            NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(tex, "GalleryTest", "Image.png", (success, path) => Debug.Log("Media save result: " + success + " " + path));

            Debug.Log("Permission result: " + permission);

            // To avoid memory leaks
            //Destroy(tex);

            OnComplete?.Invoke(filePath);
        }

        public void SaveInPhotos()
        {
            StartCoroutine(SaveRoutine());
        }

        Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            RenderTexture rt = new RenderTexture(targetX, targetY, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            Texture2D result = new Texture2D(targetX, targetY);
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }

        public Texture2D CopyTexture(Texture2D originalTexture)
        {
            //Texture2D result = new Texture2D(originalTexture.width, originalTexture.height, TextureFormat.ARGB32, false);
            Texture2D result = duplicateTexture(originalTexture);
            result.SetPixels(originalTexture.GetPixels());
            result.Apply();

            return result;
        }

        Texture2D duplicateTexture(Texture2D source)
        {
            RenderTexture renderTex = RenderTexture.GetTemporary(
                        source.width,
                        source.height,
                        0,
                        RenderTextureFormat.Default,
                        RenderTextureReadWrite.Linear);

            Graphics.Blit(source, renderTex);
            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTex;
            Texture2D readableText = new Texture2D(source.width, source.height);
            readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
            readableText.Apply();
            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTex);
            return readableText;
        }

        public Texture2D MergeTextures(Texture2D background, Texture2D watermark)
        {
            int startX = 0;
            int startY = 0;

            for (int x = startX; x < background.width; x++)
            {
                for (int y = startY; y < background.height; y++)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x, y);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    background.SetPixel(x, y, final_color);
                }
            }

            background.Apply();
            return background;
        }

        public Texture2D AddWatermark(Texture2D background, Texture2D watermark)
        {
            int startX = 0;
            int startY = background.height - watermark.height;

            for (int x = startX; x < background.width; x++)
            {
                for (int y = startY; y < background.height; y++)
                {
                    Color bgColor = background.GetPixel(x, y);
                    Color wmColor = watermark.GetPixel(x - startX, y - startY);

                    Color final_color = Color.Lerp(bgColor, wmColor, wmColor.a / 1.0f);

                    background.SetPixel(x, y, final_color);
                }
            }

            background.Apply();
            return background;
        }
    }
}