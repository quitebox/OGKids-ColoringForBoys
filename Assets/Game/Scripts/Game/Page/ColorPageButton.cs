using System.IO;
using DanielLochner.Assets.SimpleScrollSnap;
using PaintCraft.Canvas;
using PaintCraft.Canvas.Configs;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ColorPageButton : MonoBehaviour
    {
        [SerializeField] public PageConfig m_PageConfig;
        private Image m_RegionImage;
        private Image m_OutlineImage;

        private Texture2D m_Texture = null;

        private void OnEnable()
        {
            if (m_RegionImage == null)
            {
                m_RegionImage = transform.GetChild(0).GetComponent<Image>();
                m_RegionImage.raycastTarget = false;
            }

            if (m_OutlineImage == null)
            {
                m_OutlineImage = transform.GetChild(0).GetChild(0).GetComponent<Image>();
                m_OutlineImage.raycastTarget = false;
            }

            UpdateUI();
        }

        private void OnDisable()
        {
            if (m_Texture != null)
            {
                Destroy(m_RegionImage.sprite.texture);
                Destroy(m_Texture);
            }
        }

        public void UpdateUI()
        {
            if (m_PageConfig == null)
                return;

            if (m_PageConfig is ColoringPageConfig)
                m_OutlineImage.sprite = (m_PageConfig as ColoringPageConfig).Icon;


            if (File.Exists(m_PageConfig.IconSavePath) && m_PageConfig.name != null)
            {
                m_Texture = new Texture2D(512, 512, TextureFormat.RGB24, false);
                if (m_Texture.LoadImage(File.ReadAllBytes(m_PageConfig.IconSavePath)))
                {
                    m_Texture.Apply(false, true);
                    m_RegionImage.sprite = Sprite.Create(m_Texture, new Rect(0, 0, m_Texture.width, m_Texture.height), new Vector2(0.5f, 0.5f));
                }
            }
        }

        public void SelectPage()
        {
            AppData.SelectedPageConfig = m_PageConfig;
        }
    }
}