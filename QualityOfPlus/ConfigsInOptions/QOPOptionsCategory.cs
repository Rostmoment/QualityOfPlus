using MTM101BaldAPI.OptionsAPI;
using QualityOfPlus.Interfaces;
using Rewired.Utils.Classes.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.ConfigsInOptions
{
    class QOPOptionsCategory
    {
        private const int UNITS_PER_OBJECT = 60;
        private const int MAX_WORDS_PER_LINE = 12;
        private const int MIN_WORLDS_PER_LINE = 3;

        public string Name => qopCategory.Name;
        public int ObjectCount => objects.Count;
        public int TotalUnits => UNITS_PER_OBJECT * ObjectCount;

        private GameObject parent;
        private Image mask;
        private QOPCategory qopCategory;
        private CustomOptionsCategory optionsCategory;

        private int y = 60;
        private List<GameObject> objects = new List<GameObject>();
        private List<float> baseYPositions = new List<float>();

        private Dictionary<IOptionsToggleableFeature, MenuToggle> toggleables = new Dictionary<IOptionsToggleableFeature, MenuToggle>();

        public QOPOptionsCategory(GameObject parent, Image mask, QOPCategory qop, CustomOptionsCategory optionsCategory)
        {
            this.parent = parent;
            this.mask = mask;
            this.qopCategory = qop;
            this.optionsCategory = optionsCategory;
        }

        public void Build()
        {
            foreach (QOPFeature feature in qopCategory.Features)
            {
                if (feature is IOptionsToggleableFeature toggleableFeature)
                    AddToggle(toggleableFeature, feature);
            }
        }

        public void AddToggle(IOptionsToggleableFeature toggleable, QOPFeature feature)
        {
            MenuToggle toggle = optionsCategory.CreateToggle(feature.ID, toggleable.OptionToggleText, toggleable.IsEnabled(), new Vector3(100, y), QOPOptionsMenu.MASK_SIZE_X - 80);
            optionsCategory.AddTooltip(toggle, WrapDescription(toggleable.OptionToggleDescription));

            toggle.transform.SetParent(mask.transform, false);

            baseYPositions.Add(y);
            y -= UNITS_PER_OBJECT;

            toggleables.Add(toggleable, toggle);
            objects.Add(toggle.gameObject);
        }

        public void OnApplyButtonPressed()
        {
            foreach (var toggleable in toggleables)
                toggleable.Key.TrySetActive(toggleable.Value.Value);
        }

        public void SetActive(bool active) => parent.SetActive(active);
        public void ApplyScroll(float offset)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                Vector3 vector = objects[i].transform.localPosition;
                vector.y = baseYPositions[i] + offset;
                objects[i].transform.localPosition = vector;
            }
        }


        private string WrapDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return description;

            description = description.Replace("\r\n", "\n").Replace("\r", "\n");

            var resultLines = new List<string>();

            string[] paragraphs = description.Split('\n');

            foreach (string paragraph in paragraphs)
            {
                if (string.IsNullOrWhiteSpace(paragraph))
                {
                    resultLines.Add(string.Empty);
                    continue;
                }


                string[] words = Regex.Split(paragraph.Trim(), @"(?<=\S)\s+");

                var currentLine = new StringBuilder();
                int wordCount = 0;

                foreach (string word in words)
                {
                    if (string.IsNullOrWhiteSpace(word))
                        continue;

                    bool isFirstWordOnLine = wordCount == 0;

                    if (!isFirstWordOnLine)
                    {
                        if (wordCount >= MAX_WORDS_PER_LINE)
                        {
                            resultLines.Add(currentLine.ToString().TrimEnd());
                            currentLine.Clear();
                            wordCount = 0;
                            isFirstWordOnLine = true;
                        }
                        else if (wordCount >= MIN_WORLDS_PER_LINE)
                        {
                            string prevLine = currentLine.ToString().TrimEnd();
                            char lastChar = prevLine.Length > 0 ? prevLine[prevLine.Length - 1] : '\0';

                            bool endsWithSentence = lastChar == '.' || lastChar == '!' || lastChar == '?';
                            bool endsWithComma = lastChar == ',';

                            if (endsWithSentence || endsWithComma)
                            {
                                resultLines.Add(prevLine);
                                currentLine.Clear();
                                wordCount = 0;
                                isFirstWordOnLine = true;
                            }
                        }
                    }

                    if (!isFirstWordOnLine)
                        currentLine.Append(' ');

                    currentLine.Append(word);
                    wordCount++;
                }

                if (currentLine.Length > 0)
                    resultLines.Add(currentLine.ToString().TrimEnd());
            }

            return string.Join("\n", resultLines);
        }
    }
}