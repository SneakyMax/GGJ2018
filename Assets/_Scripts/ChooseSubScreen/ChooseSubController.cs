using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using XInputDotNetPure;
using Random = UnityEngine.Random;

namespace Depth.ChooseSubScreen
{
    public class ChooseSubController : MonoBehaviour
    {
        public int MainLevelIndex;
        public SubInfo[] Subs;
        public GameObject Loading;
        public RectTransform[] Areas;

        public Image FadeOverlay;
        public float FadeTime = 0.75f;

        private bool[] canGoLeftOrRight;
        private SubInfo[] currentSubInfos;

        private bool[] joined;

        private bool isStarting;

        private Image[] middleImage;
        private Image[] rightOneImage;
        private Image[] rightTwoImage;
        private Image[] leftOneImage;
        private Image[] leftTwoImage;

        private Text[] subNames;
        private FormattableText[] subAbilities;
        private FormattableText[] subInfos;

        public void Awake()
        {
            canGoLeftOrRight = new bool[4];
            currentSubInfos = new SubInfo[4];
            joined = new bool[4];
            FadeOverlay.color = new Color(FadeOverlay.color.r, FadeOverlay.color.g, FadeOverlay.color.b, 1);

            middleImage = new Image[4];
            rightOneImage = new Image[4];
            rightTwoImage = new Image[4];
            leftOneImage = new Image[4];
            leftTwoImage = new Image[4];

            subNames = new Text[4];
            subAbilities = new FormattableText[4];
            subInfos = new FormattableText[4];

            for (var player = 0; player < 4; player++)
            {
                var area = Areas[player];
                var imagePositions = area.Find("Image Positions");
                middleImage[player] = imagePositions.Find("Middle").GetComponent<Image>();
                rightOneImage[player] = imagePositions.Find("Right 1").GetComponent<Image>();
                rightTwoImage[player] = imagePositions.Find("Right 2").GetComponent<Image>();
                leftOneImage[player] = imagePositions.Find("Left 1").GetComponent<Image>();
                leftTwoImage[player] = imagePositions.Find("Left 2").GetComponent<Image>();

                var infoArea = area.Find("Info Background").Find("Texts").Find("Padding");
                subNames[player] = infoArea.Find("Sub Name").GetComponent<Text>();
                subAbilities[player] = infoArea.Find("Ability Text").GetComponent<FormattableText>();
                subInfos[player] = infoArea.Find("Info Text").GetComponent<FormattableText>();
            }
        }

        public void Start()
        {
            FadeOverlay.DOFade(0, FadeTime);
            for (var player = 0; player < 4; player++)
            {
                DoJoin(player);
                // PressStarts[player].SetActive(true));
            }
        }

        public void Update()
        {
            // Join();
            ChooseSub();
            CheckStart();
        }

        private void CheckStart()
        {
            for (var player = 0; player < 4; player++)
            {
                if (isStarting)
                    return;
                var state = GamePad.GetState((PlayerIndex) player);
                if (state.Buttons.Start == ButtonState.Pressed)
                {
                    isStarting = true;
                    StartCoroutine(StartStarting());
                }
            }
        }

        private IEnumerator StartStarting()
        {
            var choices = PlayerSubChoices.Instance;
            for (var player = 0; player < 4; player++)
            {
                choices.Set(player, currentSubInfos[player]);
            }

            FadeOverlay.DOFade(1, FadeTime);
            yield return new WaitForSeconds(FadeTime);
            var asyncLoad = SceneManager.LoadSceneAsync(MainLevelIndex, LoadSceneMode.Single);
            asyncLoad.allowSceneActivation = true;

            Loading.SetActive(true);
            yield return asyncLoad;
        }

        private void Join()
        {
            for (var player = 0; player < 4; player++)
            {
                var state = GamePad.GetState((PlayerIndex) player);
                if (state.Buttons.Start == ButtonState.Pressed)
                {
                    DoJoin(player);
                }
            }
        }

        private void DoJoin(int player)
        {
            joined[player] = true;
            //PressStarts[player].SetActive(false);
            var subIndex = Random.Range(0, Subs.Length);
            currentSubInfos[player] = Subs[subIndex];
            RecalcSubData(player, subIndex);
            CameraRenderers.Instance.Show(currentSubInfos[player].gameObject, player);
        }

        private void ChooseSub()
        {
            for (var player = 0; player < 4; player ++)
            {
                if (!joined[player])
                    continue;

                var inputState = GamePad.GetState((PlayerIndex) player);

                var x = inputState.ThumbSticks.Left.X;
                if (x < 0.25 && x > -0.25)
                {
                    canGoLeftOrRight[player] = true;
                }
                else if (x > 0.5 && canGoLeftOrRight[player])
                {
                    canGoLeftOrRight[player] = false;
                    NextSub(player);
                }
                else if (x < -0.5 && canGoLeftOrRight[player])
                {
                    canGoLeftOrRight[player] = false;
                    PreviousSub(player);
                }
            }
        }

        private int GetIndex(int desiredIndex)
        {
            if (desiredIndex < 0)
                return desiredIndex + Subs.Length;
            return desiredIndex % Subs.Length;
        }

        private void NextSub(int player)
        {
            SoundManager.PlaySound("ship_select_R");
            var currentSub = currentSubInfos[player];
            var currentIndex = new List<SubInfo>(Subs).IndexOf(currentSub);
            var nextIndex = GetIndex(currentIndex + 1);
            var nextSub = Subs[nextIndex];

            RecalcSubData(player, nextIndex);

            currentSubInfos[player] = nextSub;

            CameraRenderers.Instance.Show(nextSub.gameObject, player);
        }

        private void RecalcSubData(int player, int currentIndex)
        {
            middleImage[player].sprite = Subs[GetIndex(currentIndex)].SubImage;
            leftOneImage[player].sprite = Subs[GetIndex(currentIndex - 1)].SubImage;
            leftTwoImage[player].sprite = Subs[GetIndex(currentIndex - 2)].SubImage;
            rightOneImage[player].sprite = Subs[GetIndex(currentIndex + 1)].SubImage;
            rightTwoImage[player].sprite = Subs[GetIndex(currentIndex + 2)].SubImage;

            subNames[player].text = Subs[GetIndex(currentIndex)].Name;
            subAbilities[player].Format(Subs[GetIndex(currentIndex)].Ability);
            subInfos[player].Format(Subs[GetIndex(currentIndex)].Discription);
        }

        private void PreviousSub(int player)
        {
            SoundManager.PlaySound("ship_select_L");
            var currentSub = currentSubInfos[player];
            var currentIndex = new List<SubInfo>(Subs).IndexOf(currentSub);
            var nextIndex = GetIndex(currentIndex - 1);
            var nextSub = Subs[nextIndex];

            RecalcSubData(player, nextIndex);

            currentSubInfos[player] = nextSub;

            CameraRenderers.Instance.Show(nextSub.gameObject, player);
        }
    }
}

