using MobileTools.Utilities;
using UnityEngine;

namespace MobileTools
{
    public class LocallizationManager : Manager<LocallizationManager>
    {        
        public int GetCurrentLanguateIndex
        {
            get { return GetCurrentLanguageIndex(); }
        }        

        public void SetCurrentLanguage(string language)
        {
            switch (language)
            {
                case "English":
                    GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.English);
                    break;

                case "French":
                    GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.French);
                    break;

                case "Spanish":
                    GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Spanish);
                    break;

                case "Portuguese":
                    GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.Portuguese);
                    break;

                default:
                    GleyLocalization.Manager.SetCurrentLanguage(SupportedLanguages.English);
                    break;
            }

            Debug.Log($"Setiing {language}");
        }

        private int GetCurrentLanguageIndex()
        {
            int result = 0;

            if (GleyLocalization.Manager.GetCurrentLanguage() == SupportedLanguages.English)
                result = 0;
            else if (GleyLocalization.Manager.GetCurrentLanguage() == SupportedLanguages.French)
                result = 1;
            else if (GleyLocalization.Manager.GetCurrentLanguage() == SupportedLanguages.Spanish)
                result = 2;
            else if (GleyLocalization.Manager.GetCurrentLanguage() == SupportedLanguages.Portuguese)
                result = 3;            

            return result;
        }
    }
}