using System;
using UnityEngine;

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
    [SerializeField] private int totalCoins = 0;

    [SerializeField] private int currentPlayerIndex = 0; // Index of the current player
    public Player[] players; // Array of players in the game

    // Event to signal turn changes
    public event Action<int> OnTurnChanged;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Initialize players
        players = new Player[]
        {
            new Player(PlayerTurn.PlayerOne),
            new Player(PlayerTurn.PlayerTwo)
        };
    }

    // Method to end the current turn and start the next turn
    public void SwitchToNextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;
        OnTurnChanged?.Invoke(currentPlayerIndex);
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
    public int score; // Player's score

    [Header("Coins")]
    public int redCoin; // Number of red coins collected
    public int blackCoin; // Number of black coins collected
    public int whiteCoin; // Number of white coins collected

    public Player(PlayerTurn playerType)
    {
        this.PlayerType = playerType;
        this.score = 0; // Initialize score to zero
        this.redCoin = 0;
        this.blackCoin = 0;
        this.whiteCoin = 0;
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