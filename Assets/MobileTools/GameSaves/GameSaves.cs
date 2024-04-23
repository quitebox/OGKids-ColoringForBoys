using System;
using UnityEngine;
using MobileTools.Utilities;
using UnityEngine.Events;

namespace MobileTools
{
    public class GameSaves : Manager<GameSaves>
    {
        [Serializable]
        public class SavedValues
        {
            public bool MusicIsOn = true;
            public bool SfxIsOn = true;
            public bool VibrationIsOn = true;
            public bool NotificationIsOn = true;
            public bool HasRemoveAds = false;
            public bool HasPurchasedSubscription = false;
            public bool HasUnlockedFullApp = false;
        }

        public SavedValues Value { get { return m_SavedValues; } }
        private SavedValues m_SavedValues;

        private string fileName = "GameSaves";
        private string savePath = string.Empty;

        [Space]
        [Header("Events")]
        public UnityEvent<SavedValues> OnLoad;
        public UnityEvent<SavedValues> OnSave;

        public override void Awake()
        {
            base.Awake();

            savePath = Application.persistentDataPath + "/" + fileName;

            Load();
        }

        public void Save()
        {
            SaveManager.Instance.Save(m_SavedValues, savePath, SaveComplete, true);
        }

        private void SaveComplete(SaveResult result, string message)
        {
            if (result == SaveResult.Error)
                Debug.Log($"Save Error: {message}");
            else
            {
                Debug.Log($"Save Complete");

                OnSave?.Invoke(m_SavedValues);

                Load();
            }
        }

        private void Load()
        {
            SaveManager.Instance.Load<SavedValues>(savePath, LoadComplete, true);
        }

        private void LoadComplete(SavedValues data, SaveResult result, string message)
        {
            if (result == SaveResult.Success)
            {
                m_SavedValues = data;
                OnLoad?.Invoke(m_SavedValues);
            }

            if (result == SaveResult.Error || result == SaveResult.EmptyData)
                m_SavedValues = new SavedValues();
        }

        public void SetRemoveAds(bool value)
        {
            m_SavedValues.HasRemoveAds = value;
            Save();
        }

        public void SetSubscription(bool value)
        {
            m_SavedValues.HasPurchasedSubscription = value;
            Save();
        }

        public void UnlockFullApp(bool value) {

            m_SavedValues.HasUnlockedFullApp = value;
            Save();
        }
    }
}