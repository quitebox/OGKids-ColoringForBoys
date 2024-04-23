using PaintCraft.Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game
{
    public class BrushSounds : MonoBehaviour
    {
        [Header("Brush Config")]
        [SerializeField] private LineConfig m_LineConfig;

        [Space]
        [Header("SFX Types")]
        [SerializeField] private SoundManager.SFX_TYPE m_BrushSFXType;
        [SerializeField] private SoundManager.SFX_TYPE m_PencilSFXType;
        [SerializeField] private SoundManager.SFX_TYPE m_CrayonSFXType;
        [SerializeField] private SoundManager.SFX_TYPE m_PaintBuketSFXType;
        [SerializeField] private SoundManager.SFX_TYPE m_EraserSFXType;

        private Touch touch;
        private int touchCount;

        private void Update()
        {
            touchCount = Input.touchCount;
            if (touchCount > 0)
            {
                touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began && SoundManager.Instance != null  && !EventSystem.current.IsPointerOverGameObject(0))
                {
                    switch (m_LineConfig.Brush.name)
                    {
                        case "Brush Tool":
                            SoundManager.Instance.Play(m_BrushSFXType);
                            break;

                        case "Pencil Tool":
                            SoundManager.Instance.Play(m_PencilSFXType);
                            break;

                        case "Crayon Tool":
                            SoundManager.Instance.Play(m_CrayonSFXType);
                            break;

                        case "Bucket Tool":
                            SoundManager.Instance.Play(m_PaintBuketSFXType);
                            break;

                        case "Eraser Tool":
                            SoundManager.Instance.Play(m_EraserSFXType);
                            break;

                    }                
                }
            }
        }       
    }
}