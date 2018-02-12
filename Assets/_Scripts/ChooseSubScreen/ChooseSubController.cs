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
        public GameObject[] PressStarts;
        public GameObject Loading;

        public Image FadeOverlay;
        public float FadeTime = 0.75f;

        private bool[] canGoLeftOrRight;
        private SubInfo[] currentSubInfos;

        private bool[] joined;

        private bool isStarting;

        public void Awake()
        {
            canGoLeftOrRight = new bool[4];
            currentSubInfos = new SubInfo[4];
            joined = new bool[4];
            FadeOverlay.color = new Color(FadeOverlay.color.r, FadeOverlay.color.g, FadeOverlay.color.b, 1);
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
                if (state.Buttons.Back == ButtonState.Pressed)
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
            PressStarts[player].SetActive(false);
            currentSubInfos[player] = Subs[Random.Range(0, Subs.Length)];
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

        private void NextSub(int player)
        {
            var currentSub = currentSubInfos[player];
            var currentIndex = new List<SubInfo>(Subs).IndexOf(currentSub);
            var nextIndex = (currentIndex + 1) % Subs.Length;
            var nextSub = Subs[nextIndex];

            currentSubInfos[player] = nextSub;

            CameraRenderers.Instance.Show(nextSub.gameObject, player);
        }

        private void PreviousSub(int player)
        {
            var currentSub = currentSubInfos[player];
            var currentIndex = new List<SubInfo>(Subs).IndexOf(currentSub);
            var nextIndex = currentIndex - 1;
            if (nextIndex < 0)
                nextIndex = Subs.Length - 1;
            var nextSub = Subs[nextIndex];

            currentSubInfos[player] = nextSub;

            CameraRenderers.Instance.Show(nextSub.gameObject, player);
        }
    }
}

