using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections; // Required for Coroutines

public class TicTacToeController : MonoBehaviour
{
    [Header("Board Setup")]
    [SerializeField] private TicTacToeCell[] allCells;

    [Header("Turn UI Objects")]
    [SerializeField] private GameObject xTurnObject;
    [SerializeField] private GameObject oTurnObject;

    [Header("Win UI Objects")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject xWinObject;
    [SerializeField] private GameObject oWinObject;
    [SerializeField] private GameObject tieObject;
    //[SerializeField] private TMP_Text resultText;

    [Header("Settings")]
    public Sprite xSprite;
    public Sprite oSprite;
    [SerializeField] private float restartDelay = 2.0f; // Delay in seconds


    private const int BOARD_SIZE = 9;
    private const int WIN_PATTERN_COUNT = 8;


    private Seed _currentTurn = Seed.X;
    private Seed[] _boardData = new Seed[BOARD_SIZE];
    private GameState _gameState;

    private static readonly int[,] WinPatterns =
{
    {0,1,2}, {3,4,5}, {6,7,8},
    {0,3,6}, {1,4,7}, {2,5,8},
    {0,4,8}, {2,4,6}
};

    //private bool _isGameOver = false;

    private void Start()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        _gameState = GameState.Playing;
        //_isGameOver = false;
        _currentTurn = Seed.X;

        winPanel.SetActive(false);
        xWinObject.SetActive(false);
        oWinObject.SetActive(false);

        for (int i = 0; i < BOARD_SIZE; i++)
        {
            _boardData[i] = Seed.Empty;
            allCells[i].Initialize(i, this);
        }
        UpdateTurnUI();
    }

    public void HandleCellClick(int index, TicTacToeCell cell)
    {
        if (_gameState != GameState.Playing || _boardData[index] != Seed.Empty) return;

        _boardData[index] = _currentTurn;
        Sprite moveSprite = (_currentTurn == Seed.X) ? xSprite : oSprite;
        cell.UpdateVisual(moveSprite);

        if (CheckWin(_currentTurn))
        {
            EndGame(_currentTurn);
        }
        else if (IsBoardFull())
        {
            EndGame(Seed.Empty);
        }
        else
        {
            _currentTurn = (_currentTurn == Seed.X) ? Seed.O : Seed.X;
            UpdateTurnUI();
        }
    }

    private void UpdateTurnUI()
    {
        xTurnObject.SetActive(_currentTurn == Seed.X);
        oTurnObject.SetActive(_currentTurn == Seed.O);
    }

    private bool CheckWin(Seed seed)
    {
        for (int i = 0; i < WinPatterns.GetLength(0); i++)
        {
            if (_boardData[WinPatterns[i, 0]] == seed &&
                _boardData[WinPatterns[i, 1]] == seed &&
                _boardData[WinPatterns[i, 2]] == seed)
                return true;
        }
        return false;
    }

    private bool IsBoardFull()
    {
        foreach (var s in _boardData) if (s == Seed.Empty) return false;
        return true;
    }

    private void EndGame(Seed winner)
    {
        _gameState = GameState.GameOver;
        winPanel.SetActive(true);

        if (winner == Seed.Empty)
        {
            //resultText.text = "DRAW!";
            xTurnObject.SetActive(false);
            oTurnObject.SetActive(false);
            xWinObject.SetActive(false);
            oWinObject.SetActive(false);
            tieObject.SetActive(true);
        }
        else
        {
            //resultText.text = "WINS!";
            xTurnObject.SetActive(false);
            oTurnObject.SetActive(false);
            xWinObject.SetActive(winner == Seed.X);
            oWinObject.SetActive(winner == Seed.O);
        }

        // Start the auto-restart timer
        StartCoroutine(RestartAfterDelay());
    }

    // Coroutine to wait and then reload the scene
    private IEnumerator RestartAfterDelay()
    {
        yield return new WaitForSeconds(restartDelay);
        RestartGame();
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}