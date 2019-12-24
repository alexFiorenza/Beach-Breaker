using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public enum GameState
{
    gameOver,
    inGame,
    settings,
    menu,
    main
}
public class GUIManager : MonoBehaviour
{
    public static GUIManager sharedInstance;
    public Text movesText,scoreText;
    private int moveCounter;
    private int score;
    [SerializeField]
    private string[] scenesIdx;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private Animator animSettings;
    private const string ANIM_BOOL_DISSAPEAR = "Disappear";
    public GameState currentGameState;
    private bool instantied;
    private RectTransform transformCanvas;
    private Scene currentScene;
    public bool hasWon;
    public int extraPoints;
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
            scoreText.text = score.ToString();
        }
    }
    public int MoveCounter //Variable autocomputada
    {
        get { return moveCounter; }
        set
        {
            moveCounter = value;
            movesText.text = "Moves: " + MoveCounter;
            if (moveCounter <= 0)
            {
                moveCounter = 0;
                StartCoroutine(GameOver());
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
            
        }
        if (currentScene.name == scenesIdx[0])
        {
            score = 0;
            moveCounter = 40;
            movesText.text = "Moves: " + moveCounter;
            scoreText.text = score.ToString();
            currentGameState = GameState.inGame;
            transformCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        }
    }

    private IEnumerator GameOver()
    {
        yield return new WaitUntil(() => !BoardManager.sharedInstance.isShifting);
        yield return new WaitForSeconds(0.5f);
        currentGameState = GameState.gameOver;
        SceneManager.LoadScene(scenesIdx[1]);
    }
    private void Awake()
    {
        
    }

    public void CloseMenu()
    {
        StartCoroutine(WaitUntilAnimation(false));
        currentGameState = GameState.inGame;
    }
    public void StartMenu()
    {
        StartCoroutine(WaitUntilAnimation(true));
        currentGameState = GameState.menu;
    }
    public void RestartLvl()
    {
        currentGameState = GameState.inGame;
        SceneManager.LoadScene(scenesIdx[0]);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += scenewasLoaded;
    }
    private void scenewasLoaded(Scene scene, LoadSceneMode arg1)
    {
        currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene);
    }
    public void MainMenu()
    {
        currentGameState = GameState.main;
        SceneManager.LoadScene(scenesIdx[2]);
    }
    private IEnumerator WaitUntilAnimation(bool entrance=false)
    {
        if (entrance)
        {
            settings.SetActive(true);
            animSettings.SetBool(ANIM_BOOL_DISSAPEAR, false);
            yield return null;
        }
        else
        {
            animSettings.SetBool(ANIM_BOOL_DISSAPEAR, true);
            yield return new WaitForSeconds(1f);
            settings.SetActive(false);
        }
    }
    private void Update()
    {
      
    }
}
