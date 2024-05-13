using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton instance
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<GameManager>();
            }

            return _instance;
        }
    }

    // Variables
    [SerializeField] GameMode gameMode = GameMode.Classic; // Default GameMode is Classic
    [SerializeField] Text whiteTextRenderer;
    [SerializeField] Text blackTextRenderer;
    [SerializeField] public int currentPlayerIndex = 0; // Index of the current player
    public Player[] players; // Array of players in the game

    // Events
    public event Action<int> OnTurnChanged;
    public event Action<Player, CoinType> OnCoinCollected;
    public event Action OnStrikerFoul;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        players[currentPlayerIndex].isPlayerTurn = true;
    }

    // Method to get the current player
    public Player GetCurrentPlayer()
    {
        return players[currentPlayerIndex];
    }

    // Method to end the current turn and start the next turn
    public void SwitchToNextPlayer()
    {
        players[currentPlayerIndex].isPlayerTurn = false;
        players[currentPlayerIndex].isQueenCoveringMove = false; // Reset this flag for Previous Player
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        players[currentPlayerIndex].isPlayerTurn = true;
        OnTurnChanged?.Invoke(currentPlayerIndex);
    }

    public Transform GetNextPlayerResetPosition()
    {
        int c = (currentPlayerIndex + 1) % players.Length;
        return players[c].startPoint;
    }

    // Method to collect coins for the current player
    public void AddCoinToCurrentPlayer(CoinType type)
    {
        Player currentPlayer = GetCurrentPlayer();
        currentPlayer.CollectCoin(type);
        OnCoinCollected?.Invoke(currentPlayer, type); // Coin Collect Event
    }

    public void OnFoul()
    {
        OnStrikerFoul?.Invoke();
    }
}

public enum CoinType
{
    Red,
    Black,
    White
}

// Enum for player turns
public enum PlayerTurn
{
    PlayerOne,
    PlayerTwo,
    PlayerThree,
    PlayerFour
}

// Enum for Game Modes
public enum GameMode
{
    Classic,
    Disk
}

// Player class to represent each player
[System.Serializable]
public class Player
{
    private GameManager gameManager;
    public PlayerTurn PlayerType;
    public Transform startPoint;
    public int score; // Player's score

    [Header("Coins")]
    public int redCoin; // Number of red coins collected
    public int blackCoin; // Number of black coins collected
    public int whiteCoin; // Number of white coins collected

    [Header("Coin UI")]
    public TMPro.TextMeshProUGUI redCoinText; // UI of red coins collected
    public TMPro.TextMeshProUGUI blackCoinText; // UI of black coins collected
    public TMPro.TextMeshProUGUI whiteCoinText; // UI of white coins collected
    public bool isPlayerTurn;
    public bool isQueenCoveringMove;
    public Sprite StrikerImage;

    public Player(PlayerTurn playerType, Transform startPoint, bool isPlayerTurn, Sprite StrikerImage)
    {
        PlayerType = playerType;
        this.startPoint = startPoint;
        score = 0;
        redCoin = 0;
        blackCoin = 0;
        whiteCoin = 0;
        this.isPlayerTurn = isPlayerTurn;
        isQueenCoveringMove = false;
        this.StrikerImage = StrikerImage;
    }

    // Method to set coin counts
    public void SetCoinCounts(int red, int black, int white)
    {
        redCoin = red;
        blackCoin = black;
        whiteCoin = white;
    }

    public void UpdateCoinTextsUI()
    {
        redCoinText.text = redCoin.ToString();
        blackCoinText.text = blackCoin.ToString();
        whiteCoinText.text = whiteCoin.ToString();
    }

    // Method to handle coin functionality based on coin type
    public void CollectCoin(CoinType type)
    {
        switch (type)
        {
            case CoinType.Red:
                isQueenCoveringMove = true;
                break;
            case CoinType.Black:
                blackCoin++;
                if (isQueenCoveringMove)
                {
                    redCoin = 1;
                }
                break;
            case CoinType.White:
                whiteCoin++;
                if (isQueenCoveringMove)
                {
                    redCoin = 1;
                }
                break;
        }
        score = redCoin + blackCoin + whiteCoin;
        UpdateCoinTextsUI();
    }

    public void OnFoul()
    {
        GameManager.Instance.OnFoul();
        if (redCoin + blackCoin + whiteCoin > 0)
        {
            if (blackCoin > 0)
            {
                blackCoin--;
            }
            else if (whiteCoin > 0)
            {
                whiteCoin--;
            }
            else if (redCoin > 0)
            {
                redCoin = 0;
            }
        }
        UpdateCoinTextsUI();
    }
}