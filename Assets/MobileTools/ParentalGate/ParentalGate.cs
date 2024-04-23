using System;
using DG.Tweening;
using MobileTools.Utilities;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MobileTools
{
    public class ParentalGate : Manager<ParentalGate>
    {
        [Header("UI Referances")]
        [SerializeField] private TextMeshProUGUI m_QuestionText;
        [SerializeField] private TextMeshProUGUI m_OptionAText;
        [SerializeField] private TextMeshProUGUI m_OptionBText;
        [SerializeField] private TextMeshProUGUI m_OptionCText;
        [SerializeField] private TextMeshProUGUI m_OptionDText;
        [SerializeField] private TextMeshProUGUI m_OptionEText;
        [SerializeField] private TextMeshProUGUI m_OptionFText;

        [Space]
        [Header("Events")]
        public UnityEvent OnCloseButtonClick;
        public UnityEvent OnCorrectAnswer;
        public UnityEvent OnWrongAnswer;

        private int numberA = 0;
        private int numberB = 0;
        private int answer = 0;

        #region Puzzle
        private void OnEnable()
        {
            GeneratePuzzle();
        }

        private void GeneratePuzzle()
        {
            numberA = UnityEngine.Random.Range(1, 9);
            numberB = UnityEngine.Random.Range(1, 9);

            answer = numberA * numberB;

            UpdateTexts($"{numberA} X {numberB} =", answer);
        }

        public void CheckAnswer(TextMeshProUGUI optionText)
        {
            if (int.Parse(optionText.text) == answer)
                OnCorrectAnswer?.Invoke();
            else
                OnWrongAnswer?.Invoke();

            Close();
        }

        public void Close()
        {
            OnCloseButtonClick?.Invoke();
        }

        #endregion

        #region UI
        private void UpdateTexts(string question, int answer)
        {
            m_QuestionText.text = question;
            m_OptionAText.text = $"{UnityEngine.Random.Range(0, 16)}";
            m_OptionBText.text = $"{UnityEngine.Random.Range(17, 19)}";
            m_OptionCText.text = $"{UnityEngine.Random.Range(20, 35)}";
            m_OptionDText.text = $"{UnityEngine.Random.Range(36, 51)}";
            m_OptionEText.text = $"{UnityEngine.Random.Range(52, 67)}";
            m_OptionFText.text = $"{UnityEngine.Random.Range(68, 99)}";

            if (answer >= 0 && answer <= 16)
                m_OptionAText.text = $"{answer}";
            if (answer >= 17 && answer <= 19)
                m_OptionBText.text = $"{answer}";
            if (answer >= 20 && answer <= 35)
                m_OptionCText.text = $"{answer}";
            if (answer >= 36 && answer <= 51)
                m_OptionDText.text = $"{answer}";
            if (answer >= 52 && answer <= 67)
                m_OptionEText.text = $"{answer}";
            if (answer >= 68 && answer <= 99)
                m_OptionFText.text = $"{answer}";

            int[] siblindIndexes = generateUniqueNumbers(0, m_OptionAText.transform.parent.parent.childCount - 1);

            // Randomize the siblings
            for (int i = 0; i < siblindIndexes.Length; i++)
                m_OptionAText.transform.parent.parent.GetChild(i).SetSiblingIndex(siblindIndexes[i]);
        }

        public int[] generateUniqueNumbers(int minValue, int maxValue)
        {
            if (minValue > maxValue)
                throw new ArgumentException("Minimal value cannot be bigger than maximal value.");

            int[] values = new int[maxValue - minValue + 1];

            for (int i = 0; i < values.Length; ++i)
                values[i] = minValue + i;

            System.Random random = new System.Random();

            for (int i = 0; i < values.Length; ++i)
            {
                int index = random.Next(values.Length);

                if (i == index)
                    continue;

                int tmp = values[i];

                values[i] = values[index];
                values[index] = tmp;
            }

            return values;
        }
        #endregion
    }
}