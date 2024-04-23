using System.Collections;
using System.Collections.Generic;
using PaintCraft.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace Game
{
    public class ToolSelector : MonoBehaviour
    {
        [System.Serializable]
        public class Tool
        {
            public Brush brush;
            public Button button;
        }


        [SerializeField] private LineConfig lineConfig;
        [SerializeField] private Tool[] tools;

        private string usedBrushName;

        [Header("Events")]
        public UnityEvent<Transform> OnToolSelect;


        [ContextMenu("Fix Buttons Name")]
        private void CorrectButtonNames()
        {
            for (int i = 0; i < tools.Length; i++)
                tools[i].button.name = tools[i].brush.name;
        }       

        private void Start()
        {
            usedBrushName = PlayerPrefs.GetString(lineConfig.name);

            //if (string.IsNullOrEmpty(usedBrushName) && tools.Length > 0)
            //{
            //    lineConfig.Brush = tools[0].brush;
            //    OnToolSelect?.Invoke(tools[0].button.transform);
            //}


            for (int i = 0; i < tools.Length; i++)
            {
                if (!string.IsNullOrEmpty(usedBrushName) && tools.Length > 0)
                {
                    if (usedBrushName.Contains(tools[i].brush.name))
                    {
                        lineConfig.Brush = tools[i].brush;
                        OnToolSelect?.Invoke(tools[i].button.transform);
                    }
                }

                int index = i;
                Transform buttonT = null;
                tools[i].button.onClick.AddListener(delegate
                {
                    if (tools[index].button.name == tools[index].brush.name)
                    {
                        lineConfig.Brush = tools[index].brush;
                        PlayerPrefs.SetString(lineConfig.name, tools[index].brush.name);
                        PlayerPrefs.Save();

                        buttonT = tools[index].button.transform;
                        OnToolSelect?.Invoke(buttonT);
                    }
                });
            }
        }
    }
}