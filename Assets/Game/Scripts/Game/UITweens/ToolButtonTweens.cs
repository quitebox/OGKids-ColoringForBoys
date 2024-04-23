using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class ToolButtonTweens : MonoBehaviour
    {
        public Ease ease;
        public List<Transform> tools;

        public void OnToolSelect(Transform t)
        {
            for (int i = 0; i < tools.Count; i++)
            {
                tools[i].DOScale(1, 0.1f);
                //tools[i].GetChild(0).gameObject.SetActive(false);                
                //tools[i].GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
                //tools[i].GetChild(0).GetComponent<Image>().enabled = true;
            }

            t.DOScale(1.13f, 0.3f).SetEase(ease).OnComplete(delegate
            {
                //t.GetChild(0).gameObject.SetActive(true);
                //t.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
                //t.GetChild(0).GetChild(0).GetComponent<Image>().DOFade(0, 0.3f).From();
                //t.GetChild(0).GetComponent<Image>().enabled = false;
            });
        }
    }
}
