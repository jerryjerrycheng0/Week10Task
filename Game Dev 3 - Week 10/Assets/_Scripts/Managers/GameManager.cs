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
        public float difficulty = 1.0f;  // Difficulty level

        [SerializeField] GameEvent restartGame;
        [SerializeField] GameEvent gameOver;
        [SerializeField] GameEvent gameWin;

        private int winScoreThreshold = 2000000; // Example win score
        private float winTimeThreshold = 60f;  // Example survival time in seconds
        [SerializeField] bool isSurvival;
        private bool isGameOver; // Tracks if the game is already over
        public bool isWin;

        protected override void Awake()
        {
            base.Awake();
            Initialisation();
        }

        void Update()
        {
            if (!isGameOver) // Only run game logic if the game is not over
            {
                ValuesClamping();
                TimeGoingDown();

                if (SceneManager.GetActiveScene().name == "scn_Level1")
                {
                    StartCounting();
                    CheckWinCondition(); // Check win condition during gameplay
                }
            }

            if (SceneManager.GetActiveScene().name == "scn_GameOver" && Input.anyKeyDown || 
                SceneManager.GetActiveScene().name == "scn_GameWin" && Input.anyKeyDown)
            {
                restartGame.Raise();
            }
        }

        private void Initialisation()
        {
            playTime = 0f;
            score = 0;
            successRate = 50; // Start at a neutral 50%
            isGameOver = false; // Reset game over state
        }

        private void TimeGoingDown()
        {
            if (SceneManager.GetActiveScene().name != "scn_MainMenu" &&
                SceneManager.GetActiveScene().name != "scn_GameOver" && 
                SceneManager.GetActiveScene().name != "scn_GameWin")
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
            difficulty += 0.1f; // Increase difficulty when a good package is collected
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
                TriggerGameOver();
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
            difficulty = 1.0f; // Reset difficulty on restart
            isGameOver = false; // Reset game over state
            isWin = false;
        }

        public void CheckWinCondition()
        {
            if (isSurvival)
            {
                if (playTime >= winTimeThreshold)
                {
                    TriggerWin();
                }
            }
            else
            {
                if (score >= winScoreThreshold)
                {
                    TriggerWin();
                }
            }
        }

        private void TriggerWin()
        {   
            isWin = true;
            gameWin.Raise();

        }

        private void TriggerGameOver()
        {
            isGameOver = true;
            gameOver.Raise();
        }
    }
}
