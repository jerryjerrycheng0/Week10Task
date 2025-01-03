using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameDevWithMarco.Managers
{
    public class MyScenemanager : Singleton<MyScenemanager>
    {
        public Animator transitionAnim;
        private string gameLevel = "scn_Level1";
        private string gameLevelSurvive = "scn_Level2";
        private string gameOver = "scn_GameOver";
        public bool isSurvive;
        [SerializeField] GameManager gameManager;

        private void Update()
        {
            if (SceneManager.GetActiveScene().name == "scn_MainMenu")
            {
                StartCoroutine(WaitAndLoadNewScene());
            }
        }

        IEnumerator WaitAndLoadNewScene()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                transitionAnim.SetTrigger("end");
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene(gameLevel);
                gameManager.isSurvival = false; // Set to non-survival mode
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                transitionAnim.SetTrigger("end");
                isSurvive = true;
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene(gameLevelSurvive);
                gameManager.isSurvival = true; // Set to survival mode
            }
        }

        public void GameOverReaction()
        {
            StartCoroutine(GameOver());
        }

        IEnumerator GameOver()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(gameOver);
        }

        public void RestartGameReaction()
        {
            StartCoroutine(RestartGame());
        }

        IEnumerator RestartGame()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            if (!isSurvive)
            {
                SceneManager.LoadScene(gameLevel);
            }
            else
            {
                SceneManager.LoadScene(gameLevelSurvive);
            }
            GameManager.Instance.RestartGame();
        }
    }
}
