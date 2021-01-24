using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public Text lifesText;
    public Text levelText;
    public Text highscoreText;

    public GameObject panelMenu;
    public GameObject panelPlay;
    public GameObject panelLevelCompleted;
    public GameObject panelGameOver;

    public GameObject playerPrefab;
    public GameObject[] levels;

    public AudioSource gameSound;
    public AudioSource gameOverSound;

    public static GameManager Instance { get; private set; }

    public enum State { MENU, INIT, PLAY, LEVELCOMPLETED, LOADLEVEL, GAMEOVER }
    State _state;
    //bool isSwitchingState;

    private GameObject _currentLevel;
    private GameObject _currentLife;
    
    private int _score;
    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            scoreText.text = "SCORE: " + _score;
        }
    }
    
    private int _level;
    public int Level
    {
        get { return _level; }
        set
        {
            _level = value;
            levelText.text = "LEVEL: " + _level;
        }
    }

      
    private int _life;
    public int Life
    {
        get { return _life; }
        set
        {
            _life = value;
            lifesText.text = "LIFES: " + _life;
        }
    }  

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }
    public void ExitClicked()
    {
        Application.Quit();
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    public void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        //isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        _state = newState;
        BeginState(newState);
        //isSwitchingState = false;
    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                Cursor.visible = true;
                highscoreText.text = "HIGHSCORE: " + PlayerPrefs.GetInt("highscore"); 
                panelMenu.SetActive(true);
                gameSound.Play();                
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Life = 1;
                if (_currentLife == null)
                {
                    _currentLife = Instantiate(playerPrefab);
                }
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVELCOMPLETED:
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    _currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }
                break;
            case State.GAMEOVER:
                if (Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", Score);
                }
                panelGameOver.SetActive(true);
                gameSound.Stop();
                gameOverSound.Play();
                break;
        }
    }

    void Update()
    {
        switch (_state)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if(Life == 0)
                {
                    SwitchState(State.GAMEOVER);
                }
                if (Input.GetButtonDown("Cancel"))
                {
                    SwitchState(State.MENU);
                }
                break;
            case State.LEVELCOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }
    }

    void EndState()
    {
        switch (_state)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                Destroy(_currentLevel);
                Destroy(_currentLife);
                panelPlay.SetActive(false);
                break;
            case State.LEVELCOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(false);   
                break;
        }
    }
}
