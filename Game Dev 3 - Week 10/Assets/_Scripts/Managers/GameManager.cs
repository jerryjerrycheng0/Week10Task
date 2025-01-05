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
        public int successRate; 
        public int lives = 5;
        public float playTime = 0;
        public float difficulty = 1.0f; 
        public float maxDifficulty = 5.0f;

        [SerializeField] GameEvent restartGame;
        [SerializeField] GameEvent gameOver;

        private int winScoreThreshold = 2000000;
        private bool isGameOver; 
        public bool isWin;

        public bool isSurvival = false; // Track if Survival Mode is enabled

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

                if (SceneManager.GetActiveScene().name == "scn_Level1" || SceneManager.GetActiveScene().name == "scn_level2")
                {
                    StartCounting();
                    CheckWinCondition(); // Check win condition during gameplay
                }

                // In Survival mode, increase score every second
                if (isSurvival)
                {
                    StartCounting();
                    CheckWinCondition();
                    SurvivalSetting();
                }
            }

            if (SceneManager.GetActiveScene().name == "scn_GameOver" && Input.anyKeyDown)
            {
                restartGame.Raise();
            }
        }

        private void SurvivalSetting()
        {
            winScoreThreshold = 10000000;
            score += 1000; // Increment score every second in Survival mode
            packageValues = new int[] { 0, 0 };
        }

        private void Initialisation()
        {
            playTime = 0f;
            score = 0;
            successRate = 50; // Start at a neutral 50%
            isGameOver = false; // Reset game over state
            isWin = false; // Reset win flag
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
            score = Mathf.Clamp(score, 0, 10000000);
            successRate = Mathf.Clamp(successRate, 0, 100);
            timeLeft = Mathf.Clamp(timeLeft, 0, 120);
            lives = Mathf.Clamp(lives, 0, 10);
            difficulty = Mathf.Clamp(difficulty, 1.0f, maxDifficulty); // Clamp difficulty here
        }

        private void StartCounting()
        {
            playTime += Time.deltaTime;
        }

        public void GreenPackLogic()
        {
            score += packageValues[0];
            successRate += 1; // Increase success rate on collecting a good package

            // Only increase difficulty if it is below the max limit
            if (difficulty < maxDifficulty)
            {
                difficulty += 0.1f;
            }
        }

        public void RedPackLogic()
        {
            if (score >= 543) score += packageValues[1];
            else score = 0;

            successRate -= 3; // Decrease success rate on collecting a bad package
            lives--;

            // Decrease difficulty when the player is damaged
            difficulty -= 0.1f;

            if (lives <= 0) TriggerGameOver();
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
            isWin = false; // Reset win flag
        }

        public void CheckWinCondition()
        {
            if (score >= winScoreThreshold && !isWin) TriggerWin(); // Ensure win is triggered only once
        }

        private void TriggerWin()
        {
            isWin = true; // Set the win flag to true so it doesn't trigger again
            gameOver.Raise();
        }

        private void TriggerGameOver()
        {
            isGameOver = true;
            gameOver.Raise();
        }
    }
}
