using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Text gameOverText;
    public Text restartText;
    public Text wonText;

    public AudioSource gameSound;
    public AudioSource gameOverSound;

    private bool gameOver;
    private bool restart;
    private bool won;

    private void Awake()
    {
        gameSound.Play();
        gameOver = false;
        restart = false;
        won = false;
        gameOverText.text = "";
        restartText.text = "";
        wonText.text = "";
    }

    private void Update()
    {
        if (gameOver)
        {
            restart = true;
        }
        if (restart)
        {
            restartText.text = "Press 'Enter' to restart";
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SceneManager.LoadScene("Game");
            }
        }
        if (won)
        {
            restart = true;
        }
    }

    public void GameOver()
    {
        gameOverText.text = "GAME OVER";
        gameOver = true;
        gameSound.Stop();
        gameOverSound.Play();
    }
    public void Win()
    {
        wonText.text = "WON";
        won = true;
    }
}
