using MTM101BaldAPI.UI;
using QualityOfPlus.BetterMenu.DarkMode;
using QualityOfPlus.BetterSeed.CopySeed;
using QualityOfPlus.Helpers.Extensions;
using QualityOfPlus.NotificationSystem;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace QualityOfPlus.Gameplay.GameplayHistory
{
    internal class GameplayHistoryMenu : Singleton<GameplayHistoryMenu>
    {
        private static string[] dontDestroy = new string[]
        {
            "BG", "BackButton", "CursorOrigin(Clone)"
        };

        private GameObject entries;
        private Image mask;
        private StandardMenuButton scrollButton;
        private float offset;

        private const float TOP_HEIGHT = 144f;
        private const float SPACING = 40;

        private const float MASK_X = -5f;
        private const float MASK_Y = 0f;
        private const float MASK_WIDTH = 560f;
        private const float MASK_HEIGHT = 350f;

        private const float SCROLL_X = 295f;
        private const float SCROLL_Y = 0f;
        private const float SCROLL_W = 25f;
        private const float SCROLL_H = 350f;
        private const float SCROLL_MAX = 150;
        private const float SCROLL_MIN = -150f;

        public override void AwakeFunction()
        {
            foreach (Transform child in transform)
                if (!dontDestroy.Contains(child.name))
                    Destroy(child.gameObject);

            SetupMask();
            SetupScrollbar();

            entries = new GameObject("Entries");
            entries.transform.SetParent(mask.transform, false);
            entries.transform.localPosition = Vector3.zero;

            for (int i = GameplayHistoryStorage.Entries.Length - 1; i >= 0; i--)
                AddEntry(GameplayHistoryStorage.Entries[i]);

            transform.Find("CursorOrigin(Clone)").SetAsLastSibling();
        }

        private void SetupPositions()
        {
            transform.Find("BG").GetComponent<RectTransform>().sizeDelta = new Vector2(640, 360);

            CursorController controller = transform.Find("CursorOrigin(Clone)").GetComponent<CursorController>();
            Vector2 min = controller.minRange;
            min.x = -80;
            controller.minRange = min;
            Vector2 max = controller.maxRange;
            max.x = 560;
            controller.maxRange = max;

            transform.Find("BackButton").transform.localPosition = new Vector3(-320, 180, 0);
        }

        private void SetupMask()
        {
            GameObject obj = new GameObject("HistoryMask");
            obj.transform.SetParent(transform, false);
            obj.transform.localPosition = new Vector3(MASK_X, MASK_Y, 0f);

            mask = obj.AddComponent<Image>();
            mask.rectTransform.sizeDelta = new Vector2(MASK_WIDTH, MASK_HEIGHT);
            mask.color = Color.white;
            obj.AddComponent<Mask>().showMaskGraphic = false;
        }

        private void SetupScrollbar()
        {
            GameObject barObj = new GameObject("ScrollBG");
            barObj.transform.SetParent(transform, false);
            barObj.transform.localPosition = new Vector3(SCROLL_X, SCROLL_Y, 0f);

            Image barImage = barObj.AddComponent<Image>();
            barImage.rectTransform.sizeDelta = new Vector2(SCROLL_W, SCROLL_H);
            barImage.color = Color.black;
            barObj.AddComponent<Outline>().effectColor = Color.white;

            GameObject btnObj = new GameObject("ScrollButton");
            btnObj.transform.SetParent(barObj.transform, false);
            btnObj.transform.localPosition = new Vector3(0f, SCROLL_MAX, 0f);

            Image btnImage = btnObj.AddComponent<Image>();
            btnImage.rectTransform.sizeDelta = new Vector2(SCROLL_W - 4f, 45f);
            btnImage.color = new Color(0.277f, 0.757f, 0.101f);
            btnImage.raycastTarget = true;
            btnObj.AddComponent<Outline>().effectColor = Color.white;

            scrollButton = btnObj.ConvertToButton<StandardMenuButton>();
        }

        private void AddEntry(GameplayHistoryEntry entry)
        {
            TextMeshProUGUI text = UIHelpers.CreateText<TextMeshProUGUI>(
                BaldiFonts.ComicSans24,
                entry.ToString(),
                entries.transform,
                new Vector2(0f, TOP_HEIGHT - SPACING * entries.transform.childCount + 10),
                false);
            text.raycastTarget = true;

            text.alignment = TextAlignmentOptions.TopLeft;
            text.raycastTarget = true;
            text.rectTransform.sizeDelta = new Vector2(MASK_WIDTH - 5, 30);

            StandardMenuButton button = text.gameObject.ConvertToButton<StandardMenuButton>(true);
            button.underlineOnHigh = true;
            button.OnPress.AddListener(() =>
            {
                MusicManager.Instance.PlaySoundEffect(
                    QOPManager.Instance.GetFeature<CopySeedFeature>().CopySound);
                GUIUtility.systemCopyBuffer = entry.seed.ToString();
                BasePlugin.Notification.ShowMessageAndHide(new NotificationData(
                    "QOP_SEED_COPIED", entry.seed.ToString(), NotificationColor.Green), 3);
            });

            if (!QOPManager.Instance.GetFeatureIfEnabled<DarkModeFeature>(out _))
                text.color = Color.black;
        }

        private void Update()
        {
            float contentHeight = GameplayHistoryStorage.Entries.Length * SPACING;
            float maxOffset = Mathf.Max(0f, contentHeight - MASK_HEIGHT);
            float trackRange = SCROLL_MAX - SCROLL_MIN;

            if (maxOffset <= 0f)
            {
                MoveButton(SCROLL_MAX);
                ApplyScroll(0f);
                return;
            }

            if (scrollButton.held)
            {
                Vector3 p = scrollButton.transform.localPosition;
                p.y = Mathf.Clamp(p.y + CursorController.Instance.movementThisFrame.y, SCROLL_MIN, SCROLL_MAX);
                scrollButton.transform.localPosition = p;
            }

            float wheel = Input.mouseScrollDelta.y;
            if (wheel != 0f && IsMouseOverMask())
            {
                float t = (SCROLL_MAX - scrollButton.transform.localPosition.y) / trackRange;
                float newT = Mathf.Clamp01(t - wheel * 0.15f);
                MoveButton(Mathf.Lerp(SCROLL_MIN, SCROLL_MAX, 1f - newT));
            }

            float tFinal = (SCROLL_MAX - scrollButton.transform.localPosition.y) / trackRange;
            ApplyScroll(tFinal * maxOffset * -1);
        }

        private void MoveButton(float y)
        {
            Vector3 p = scrollButton.transform.localPosition;
            p.y = y;
            scrollButton.transform.localPosition = p;
        }

        private void ApplyScroll(float newOffset)
        {
            offset = newOffset;
            entries.transform.localPosition = new Vector3(0f, -offset, 0f);
        }

        private bool IsMouseOverMask()
        {
            Vector2 m = CursorController.Instance.position;
            return Mathf.Abs(m.x - MASK_X) < MASK_WIDTH / 2f &&
                   Mathf.Abs(m.y - MASK_Y) < MASK_HEIGHT / 2f;
        }

        public static void OpenMenu(HideSeekMenu hide)
        {
            if (Instance.IsNullOrDestroyed())
            {
                GameObject menu = GameObject.Instantiate(SceneManager.GetActiveScene().GetRootGameObjects().First(x => x.name == "About"));
                menu.gameObject.SetActive(false);
                menu.AddComponent<GameplayHistoryMenu>().destroyOnLoad = true;
                menu.gameObject.SetActive(true);
            }
            StandardMenuButton back = Instance.transform.Find("BackButton").GetComponent<StandardMenuButton>();
            back.transitionOnPress = true;
            back.transitionTime = 0.0167f;
            back.OnPress = new UnityEngine.Events.UnityEvent();
            back.OnPress.AddListener(() =>
            {
                hide.gameObject.SetActive(true);
                Instance.gameObject.SetActive(false);
            });

            Instance.gameObject.SetActive(true);
            hide.gameObject.SetActive(false);
            Instance.SetupPositions();
        }
    }
}