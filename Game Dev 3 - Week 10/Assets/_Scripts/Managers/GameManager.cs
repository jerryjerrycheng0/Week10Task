using UnityEngine;
using UnityEngine.SceneManagement;
using GameDevWithMarco.ObserverPattern;

namespace GameDevWithMarco.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public int score;
        public float timeLeft = 60f;
        public int[] packageValues = new int[] { 12345, -5434 };
        public int successRate; // Now explicitly treated as a percentage (0–100)
        public int lives = 5;
        public float playTime = 0;

        [SerializeField] GameEvent restartGame;
        [SerializeField] GameEvent gameOver;

        protected override void Awake()
        {
            base.Awake();
            Initialisation();
        }

        void Update()
        {
            ValuesClamping();
            TimeGoingDown();
            if (SceneManager.GetActiveScene().name == "scn_Level1")
            {
                StartCounting();
            }
            else if (SceneManager.GetActiveScene().name == "scn_GameOver" && Input.anyKeyDown)
            {
                restartGame.Raise();
            }
        }

        private void Initialisation()
        {
            playTime = 0f;
            score = 0;
            successRate = 50; // Start at a neutral 50%
        }

        private void TimeGoingDown()
        {
            if (SceneManager.GetActiveScene().name != "scn_MainMenu" &&
                SceneManager.GetActiveScene().name != "scn_GameOver")
            {
                timeLeft -= Time.deltaTime;
            }
        }

        private void ValuesClamping()
        {
            score = Mathf.Clamp(score, 0, 100000000);
            successRate = Mathf.Clamp(successRate, 0, 100);
            timeLeft = Mathf.Clamp(timeLeft, 0, 120);
            lives = Mathf.Clamp(lives, 0, 10);
        }

        private void StartCounting()
        {
            playTime += Time.deltaTime;
        }

        public void GreenPackLogic()
        {
            score += packageValues[0];
            successRate += 1; // Increase success rate on collecting a good package
        }

        public void RedPackLogic()
        {
            if (score >= 543)
            {
                score += packageValues[1];
            }
            else
            {
                score = 0;
            }
            successRate -= 3; // Decrease success rate on collecting a bad package
            lives--;
            if (lives <= 0)
            {
                gameOver.Raise();
            }
        }

        public void LifePackLogic()
        {
            lives++;
        }

        public void RestartGame()
        {
            score = 0;
            successRate = 50; // Reset to neutral on restart
            lives = 5;
            playTime = 0f;
        }
    }
}
