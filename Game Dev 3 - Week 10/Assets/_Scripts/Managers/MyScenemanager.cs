﻿using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameDevWithMarco.Managers
{
    public class MyScenemanager : Singleton<MyScenemanager>
    {
        public Animator transitionAnim;
        private string gameLevel = "scn_Level1";
        private string gameOver = "scn_GameOver";
        private string gameWin = "scn_GameWin";
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
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                transitionAnim.SetTrigger("end");
                yield return new WaitForSeconds(1.5f);
                SceneManager.LoadScene(gameLevel);
            }


        }

        public void GameOverReaction()
        {
            if (gameManager.isWin == false)
            {
                StartCoroutine(GameOver());
            }
            else
            {
                StartCoroutine(GameWin());
            }
        }



        IEnumerator GameOver()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(gameOver);
        }

        IEnumerator GameWin()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(gameWin);
        }

            public void RestartGameReaction()
        {
            StartCoroutine(RestartGame());
        }

        IEnumerator RestartGame()
        {
            transitionAnim.SetTrigger("end");
            yield return new WaitForSeconds(1.5f);
            SceneManager.LoadScene(gameLevel);
            GameManager.Instance.RestartGame();
        }

    }
}
