using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets
{
    public class GameplayManager : MonoBehaviour
    {
        public event Action GameStarted;

        public static GameplayManager Instance { get; set; }

        public float TimeRemaining { get { return GameLength - (Time.time - gameStartTime); } }

        public FormattableText TimeLeftText;

        public FormattableText CountdownText;

        public bool AllowInput = false;

        public GameObject[] VictorySplashes;

        public float GameLength = 60;

        public float WinnerDuration = 10;

        private float gameStartTime;

        public bool IsGameStarted { get; private set; }
        public bool IsGameEnded { get; private set; }

        public void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            foreach (var splash in VictorySplashes)
            {
                splash.SetActive(false);
            }
            StartCoroutine(StartCountdownCo());
        }

        public IEnumerator StartCountdownCo()
        {
            TimeLeftText.Format(0, 0);
            CountdownText.gameObject.SetActive(true);
            CountdownText.Format("3");
            yield return new WaitForSeconds(1);
            CountdownText.Format("2");
            yield return new WaitForSeconds(1);
            CountdownText.Format("1");
            yield return new WaitForSeconds(1);
            CountdownText.gameObject.SetActive(false);
            GameStart();
        }

        public void GameStart()
        {
            gameStartTime = Time.time;
            AllowInput = true;
            IsGameStarted = true;

            if (GameStarted != null)
                GameStarted();
        }

        public void Update()
        {
            if (IsGameStarted && !IsGameEnded)
            {
                var remaining = TimeSpan.FromSeconds(TimeRemaining);
                TimeLeftText.Format(remaining.Minutes, remaining.Seconds);

                if (TimeRemaining <= 0)
                {
                    GameEnded();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        private void GameEnded()
        {
            IsGameEnded = true;
            StartCoroutine(GameEndedCo());
        }

        private IEnumerator GameEndedCo()
        {
            AllowInput = false;

            var winningScore = SubManager.Instance.Subs.Max(x => x.Kills);
            if (winningScore == 0)
            {
                // no winners :(
            }
            else
            {
                var winners = SubManager.Instance.Subs.Where(x => x.Kills == winningScore).Select(x => x.Player);
                foreach (var winner in winners)
                {
                    VictorySplashes[winner].SetActive(true);
                }
            }

            yield return new WaitForSeconds(WinnerDuration);

            ReloadGame();
        }

        private void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
