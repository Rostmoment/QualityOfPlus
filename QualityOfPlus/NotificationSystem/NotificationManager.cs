using MTM101BaldAPI;
using MTM101BaldAPI.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QualityOfPlus.NotificationSystem
{
    internal class NotificationManager : MonoBehaviour
    {
        private static readonly Vector2 position = new Vector2(1, 0.5f);

        private CanvasGroup group;
        private Canvas canvas;
        private Outline outline;
        private Image backgroundImage;
        private TextMeshProUGUI title;
        private TextMeshProUGUI content;

        private Coroutine activeFadeRoutine;
        private Coroutine activeHideRoutine;

        public static NotificationManager CreateInstance(string name)
        {
            GameObject gameObject = new GameObject(name);
            DontDestroyOnLoad(gameObject);

            Canvas canvas = gameObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 32767;

            gameObject.AddComponent<GraphicRaycaster>();
            CanvasScaler scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            CanvasGroup group = gameObject.AddComponent<CanvasGroup>();
            group.alpha = 0f; // По умолчанию скрыто

            GameObject bgObject = new GameObject("NotificationBackground");
            bgObject.transform.SetParent(gameObject.transform, false);

            Image bgImage = bgObject.AddComponent<Image>();
            bgImage.sprite = null;

            RectTransform bgRect = bgObject.GetComponent<RectTransform>();
            bgRect.sizeDelta = new Vector2(500, 250);

            bgRect.anchorMin = position;
            bgRect.anchorMax = position;
            bgRect.pivot = position;
            bgRect.anchoredPosition = new Vector2(-20f, 0f); 

            Outline outline = bgObject.AddComponent<Outline>();
            outline.effectColor = Color.white;

            TextMeshProUGUI titleText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.SmoothComicSans36, "", bgObject.transform, Vector3.zero);
            titleText.alignment = TextAlignmentOptions.Center;
            titleText.color = Color.white;
            titleText.name = "TitleText";

            titleText.rectTransform.anchorMin = new Vector2(0, 0.7f);
            titleText.rectTransform.anchorMax = new Vector2(1, 1f);
            titleText.rectTransform.offsetMin = Vector2.zero;
            titleText.rectTransform.offsetMax = Vector2.zero;

            TextMeshProUGUI contentText = UIHelpers.CreateText<TextMeshProUGUI>(BaldiFonts.ComicSans24, "", bgObject.transform, Vector3.zero);
            contentText.alignment = TextAlignmentOptions.Center;
            contentText.color = Color.white;
            contentText.name = "ContentText";

            contentText.rectTransform.anchorMin = new Vector2(0, 0f);
            contentText.rectTransform.anchorMax = new Vector2(1, 0.7f);
            contentText.rectTransform.offsetMin = new Vector2(10, 10);
            contentText.rectTransform.offsetMax = new Vector2(-10, -10);

            NotificationManager notification = gameObject.AddComponent<NotificationManager>();
            notification.group = group;
            notification.canvas = canvas;
            notification.outline = outline;
            notification.backgroundImage = bgImage;
            notification.title = titleText;
            notification.content = contentText;

            notification.SetHidden(true);

            return notification;
        }

        public void ShowMessage(NotificationData data)
        {
            title.text = data.Title;
            content.text = data.Content;
            backgroundImage.color = data.SpriteColor;
            outline.effectColor = data.OutlineColor;

            if (activeHideRoutine != null)
            {
                StopCoroutine(activeHideRoutine);
                activeHideRoutine = null;
            }

            StartFade(1f, 0.5f);
        }

        public void ShowMessageAndHide(NotificationData data, float hideIn)
        {
            ShowMessage(data);
            activeHideRoutine = StartCoroutine(HideAfterDelay(hideIn));
        }

        public void Hide()
        {
            if (activeHideRoutine != null)
            {
                StopCoroutine(activeHideRoutine);
                activeHideRoutine = null;
            }

            StartFade(0f, 0.5f);
        }

        private void StartFade(float targetAlpha, float duration)
        {
            if (activeFadeRoutine != null)
            {
                StopCoroutine(activeFadeRoutine);
            }

            activeFadeRoutine = StartCoroutine(FadeRoutine(targetAlpha, duration));
        }

        private IEnumerator FadeRoutine(float targetAlpha, float duration)
        {
            float startAlpha = group.alpha;
            float time = 0f;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime; 
                group.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
                yield return null;
            }

            group.alpha = targetAlpha;
            activeFadeRoutine = null;
        }

        private IEnumerator HideAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            Hide();
        }

        public void SetHidden(bool hidden)
        {
            if (activeFadeRoutine != null) 
                StopCoroutine(activeFadeRoutine);
            if (activeHideRoutine != null) 
                StopCoroutine(activeHideRoutine);

            group.alpha = hidden ? 0f : 1f;
        }
    }
}