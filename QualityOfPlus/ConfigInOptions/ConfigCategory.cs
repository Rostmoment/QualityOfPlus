using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.ConfigInOptions
{
    class ConfigCategory
    {
        private static Dictionary<ConfigEntry<bool>, MenuToggle> toggles = new Dictionary<ConfigEntry<bool>, MenuToggle>();
        private const int UNITS_PER_CONFIG = 60;
        private const float MOUSE_SCROLL_MULTIPLIER = -5;
        private const int MAX_WORDS_PER_LINE = 12;
        private const int MIN_WORLDS_PER_LINE = 3;

        public ConfigCategory(GameObject parent, Image mask)
        {
            this.parent = parent;
            this.mask = mask;
            this.y = 60;
        }
        public string Name => parent.name;

        private GameObject parent;
        private Image mask;
        private int y;
        public List<GameObject> objects = new List<GameObject>();

        public void SetActive(bool active) => parent.SetActive(active);

        public void AddToggle(ConfigOptionsMenu menu, ConfigEntry<bool> config)
        {
            if (toggles.TryGetValue(config, out MenuToggle toggle))
            {
                if (!toggle.IsNullOrDestroyed())
                    return;
            }

            toggle = menu.CreateToggle(config.Definition.Key, config.Definition.Key, config.Value, new Vector3(100, y), ConfigOptionsMenu.MASK_SIZE_X - 80);
            menu.AddTooltip(toggle, WrapDescription(config.Description.Description) + $"\nDefault: {config.DefaultValue}");
            toggle.transform.SetParent(mask.transform, false);
            y -= UNITS_PER_CONFIG;

            toggles[config] = toggle;
            objects.Add(toggle.gameObject);
        }

        public void OnApplyButtonPressed()
        {
            foreach (var data in toggles)
                data.Key.Value = data.Value.Value;
        }


        public void ScrollFor(float y)
        {
            float scroll = y * MOUSE_SCROLL_MULTIPLIER;

            foreach (GameObject gameObject in objects)
            {
                Vector3 vector = gameObject.transform.localPosition;
                vector.y += scroll;
                gameObject.transform.localPosition = vector;
            }
        }

        private int WordsInDescription(string description) => Regex.Matches(description, @"\b\w+\b").Count;

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
