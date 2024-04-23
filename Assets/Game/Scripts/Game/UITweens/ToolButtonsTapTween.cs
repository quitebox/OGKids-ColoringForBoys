using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class ToolButtonsTapTween : MonoBehaviour
    {
        public DOTweenAnimation background;
        public DOTweenAnimation icon;        

        public void OnButtonPressed()
        {
            background.DORestart();
            icon.DORestart();            
        }
    }
}
