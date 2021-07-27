using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager _instance;

    public static GameManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    [SerializeField]
    private GameObject _startScreen, _inGameUI = default;

    [SerializeField]
    private ScreenFader _screenFader = default;

    [SerializeField]
    private EndScreen _endScreen = default;


    public Faction CurrentTurn = Faction.Human;

    public bool HasLevelFinished { get; set; }

    public bool HasLevelStarted { get; set; }

    public bool IsGameOver { get; private set; }

    private void Start()
    {
        BoardManager.Instance.OnBoardInit += OnBoardInit;
        SetupScene();
    }

    private void OnBoardInit()
    {
        List<Piece> drones = EnemyManager.Instance.Pieces.Where(p => p.PieceType == typeof(Drone)).ToList();

        foreach (var drone in drones)
        {
            drone.OnTurnCompleted += CheckDroneVictoryCondition;
        }
    }

    private void CheckDroneVictoryCondition(Piece drone)
    {
        if (drone.CurrentY == 0)
        {
            string reason = string.Format("AI drone has reached human starting point at {0}:{1}", drone.CurrentX, drone.CurrentY);
            this.EndGame(Faction.AI, reason);
        }
    }

    private void SetupScene()
    {
        _startScreen.SetActive(true);
        _screenFader.gameObject.SetActive(true);
        _inGameUI.SetActive(false);
    }

    public void EndGame(Faction winner, string reason)
    {
        this.IsGameOver = true;

        _screenFader.gameObject.SetActive(true);
        _endScreen.ActivateWithDelay();
        _inGameUI.SetActive(false);
        _endScreen.UpdateText(winner, reason);
        _screenFader.FadeOn();
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void PlayLevel()
    {
        this.HasLevelStarted = true;

        _screenFader.FadeOff();
        _endScreen.gameObject.SetActive(false);
        _startScreen.SetActive(false);

        BoardManager.Instance.InitBoard();
        StartCoroutine(StartLevel());
    }

    public IEnumerator StartLevel()
    {
        yield return new WaitForSeconds(1f);
        PlayerManager.Instance.InputEnabled = true;

        _screenFader.gameObject.SetActive(false);
        _inGameUI.SetActive(true);
        PlayPlayerTurn();
    }

    public void SwitchTurn()
    {
        if (CurrentTurn == Faction.Human)
        {
            if (PlayerManager.Instance.IsTurnComplete)
            {
                PlayEnemyTurn();
            }
        }
        else if (CurrentTurn == Faction.AI)
        {
            if (EnemyManager.Instance.IsEnemyTurnComplete())
            {
                PlayPlayerTurn();
            }
        }
    }

    public void PlayPlayerTurn()
    {
        CurrentTurn = Faction.Human;
        PlayerManager.Instance.StartHumanTurn();
    }

    private void PlayEnemyTurn()
    {
        CurrentTurn = Faction.AI;
        PlayerManager.Instance.InputEnabled = false;
        UIManager.Instance.UpdateSelectedPiece(null);
        EnemyManager.Instance.StartAITurn();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}