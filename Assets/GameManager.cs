using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
    [SerializeField] private Text whiteTextRenderer;
    [SerializeField] private Text blackTextRenderer;

    [SerializeField] private int totalCoins = 0;
    [SerializeField] public int currentPlayerIndex = 0; // Index of the current player
    [SerializeField] public int previousPlayerIndex = 0; // Index of the previous player
    public Player[] players; // Array of players in the game

    // Event to signal turn changes
    public event Action<int> OnTurnChanged;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        players[currentPlayerIndex].isPlayerTurn = true;
    }

    public void UpdateScoreUI()
    {
        if (whiteTextRenderer != null && blackTextRenderer != null)
        {
            whiteTextRenderer.text = players[0].score.ToString();
            blackTextRenderer.text = players[1].score.ToString();
        }
    }

    // Method to end the current turn and start the next turn
    public void SwitchToNextPlayer()
    {
        previousPlayerIndex = currentPlayerIndex;
        players[currentPlayerIndex].isPlayerTurn = false;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        players[currentPlayerIndex].isPlayerTurn = true;
        OnTurnChanged?.Invoke(currentPlayerIndex);
    }

    public void EndTurn()
    {
        previousPlayerIndex = currentPlayerIndex;
        players[currentPlayerIndex].isPlayerTurn = false;
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        players[currentPlayerIndex].isPlayerTurn = true;
        OnTurnChanged?.Invoke(currentPlayerIndex);
    }

    public Transform GetNextPlayerResetPosition()
    {
        int c = (currentPlayerIndex + 1) % players.Length;
        Debug.Log(c);
        return players[c].startPoint;
        // OnTurnChanged?.Invoke(currentPlayerIndex);
    }

    // Method to collect coins for the current player
    public void AddCoinToCurrentPlayer(CoinType type)
    {
        Player currentPlayer = players[currentPlayerIndex];
        currentPlayer.CollectCoin(type);
        totalCoins++;
    }

    // Method to retrieve Total Coins
    public int GetTotalCoins()
    {
        return totalCoins;
    }

    // Method to get the current player's score
    public int GetCurrentPlayerScore()
    {
        return players[currentPlayerIndex].score;
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
    PlayerTwo
}

// Player class to represent each player
[System.Serializable]
public class Player
{
    public PlayerTurn PlayerType;
    public Transform startPoint;
    public int score; // Player's score

    [Header("Coins")]
    public int redCoin; // Number of red coins collected
    public int blackCoin; // Number of black coins collected
    public int whiteCoin; // Number of white coins collected
    public bool isPlayerTurn;
    public Sprite StrikerImage;

    public Player(PlayerTurn playerType, bool isPlayerTurn, Sprite StrikerImage)
    {
        this.PlayerType = playerType;
        this.startPoint = startPoint;
        this.score = 0;
        this.redCoin = 0;
        this.blackCoin = 0;
        this.whiteCoin = 0;
        this.isPlayerTurn = isPlayerTurn;
        this.StrikerImage = StrikerImage;
    }

    // Method to set coin counts
    public void SetCoinCounts(int red, int black, int white)
    {
        redCoin = red;
        blackCoin = black;
        whiteCoin = white;
    }

    // Method to increase coin count by 1 based on coin type
    public void CollectCoin(CoinType type)
    {
        switch (type)
        {
            case CoinType.Red:
                redCoin++;
                break;
            case CoinType.Black:
                blackCoin++;
                break;
            case CoinType.White:
                whiteCoin++;
                break;
        }
        score = redCoin + blackCoin + whiteCoin;
    }
}