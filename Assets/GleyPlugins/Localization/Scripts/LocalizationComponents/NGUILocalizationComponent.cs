#if EnableNGUILocalization
namespace GleyLocalization
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public class NGUILocalizationComponent : MonoBehaviour, ILocalizationComponent
    {
        public WordIDs wordID;

        /// <summary>
        /// Used for automatically refresh
        /// </summary>
        public void Refresh()
        {
            GetComponent<Label>().text = LocalizationManager.Instance.GetText(wordID);
        }

        void Start()
        {
            Refresh();
        }
    }
}
#endif
