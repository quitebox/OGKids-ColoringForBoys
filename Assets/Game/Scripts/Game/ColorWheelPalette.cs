using System.Collections;
using System.Collections.Generic;
using PaintCraft.Tools;
using UnityEngine;

namespace Game
{
    public class ColorWheelPalette : MonoBehaviour
    {
        public LineConfig lineConfig;

        public void OnColorChange(Color color)
        {
            PointColor pointColor = PointColor.White;
            pointColor.Color = color;
            lineConfig.Color = pointColor;
        }

        public void OnHueChange(Color color)
        {
            PointColor pointColor = PointColor.White;
            pointColor.Color = color;
            lineConfig.Color = pointColor;
        }

        public void SetColorToWhite()
        {
            OnColorChange(Color.white);
        }
    }
}
