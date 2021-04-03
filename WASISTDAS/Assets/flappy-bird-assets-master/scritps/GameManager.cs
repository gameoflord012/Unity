using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public delegate void GameDelegate();
    public static event GameDelegate OnGameStart;
    public static event GameDelegate OnGameOverConfirmed;

    enum PageState
    {
        None,
        GameStart,
        GameOver
    }
    PageState currentPageState =  PageState.None;

    public static GameManager Instance;

    public GameObject startPage;
    public GameObject gameOverPage;
    public Text scoreText;

    int score = 0;
    bool gameOver = true;

    public int Score { get { return score; } }
    public bool GameOver {  get { return gameOver; } }

    public Animator BlackAnimator;

    public AudioSource FlyAudio;
    public AudioSource PointAudio;
    public AudioSource DieAudio;
    public AudioSource HitAudio;

    private void Awake()
    {
        Instance = this;       
    }

    private void Start()
    {
        SetPageState(PageState.GameStart);
        scoreText.text = "";
        gameOver = true;
    }

    private void OnEnable()
    {
        TapController.OnPlayerDied += OnPlayerDied;
        TapController.OnPlayerScored += OnPlayerScored;
    }

    private void OnDisable()
    {
        TapController.OnPlayerDied -= OnPlayerDied;
        TapController.OnPlayerScored -= OnPlayerScored;
    }

    private void Update()
    {
        if(currentPageState == PageState.GameStart)
            if(Input.GetMouseButtonDown(0))
                StartGame();        
    }

    void SetPageState(PageState state)
    {
        switch(state)
        {
            case PageState.None:
                currentPageState = PageState.None;
                startPage.SetActive(false);
                gameOverPage.SetActive(false);
                break;
            case PageState.GameStart:
                currentPageState = PageState.GameStart;
                startPage.SetActive(true);
                gameOverPage.SetActive(false);
                break;
            case PageState.GameOver:
                currentPageState = PageState.GameOver;
                startPage.SetActive(false);
                gameOverPage.SetActive(true);
                break;
            default:                
                break;
        }
    }

    public void StartGame()
    {
        OnGameStart();
        score = 0;
        scoreText.text = "0";
        gameOver = false;
        SetPageState(PageState.None);
    }
    public void ConfirmGameOver()
    {
        OnGameOverConfirmed();
        scoreText.text = "";
        SetPageState(PageState.GameOver);        
    }

    void OnPlayerScored()
    {
        score++;
        scoreText.text = score.ToString();
    }

    void OnPlayerDied()
    {        
        gameOver = true;
        int savedScore = PlayerPrefs.GetInt("HighScore");
        if (savedScore < score)
            PlayerPrefs.SetInt("HighScore", score);

        StartCoroutine("PageTransition");
    }

    IEnumerator PageTransition()
    {
        BlackAnimator.SetTrigger("Hit");
        yield return new WaitForSeconds(0.15f);
        ConfirmGameOver();
    }
}
