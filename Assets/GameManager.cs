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
    [SerializeField] private int redCoins = 0;
    [SerializeField] private int blackCoins = 0;
    [SerializeField] private int whiteCoins = 0;

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

    // Method to collect coins
    public void CollectCoin(CoinType type)
    {
        switch (type)
        {
            case CoinType.Red:
                redCoins++;
                break;
            case CoinType.Black:
                blackCoins++;
                break;
            case CoinType.White:
                whiteCoins++;
                break;
        }

        // Update the current player's score
        players[currentPlayerIndex].Score++;
    }

    // Methods to retrieve coin counts
    public int GetRedCoins()
    {
        return redCoins;
    }

    public int GetBlackCoins()
    {
        return blackCoins;
    }

    public int GetWhiteCoins()
    {
        return whiteCoins;
    }

    // Method to get the current player's score
    public int GetCurrentPlayerScore()
    {
        return players[currentPlayerIndex].Score;
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
    public int Score; // Player's score

    public Player(PlayerTurn playerType)
    {
        this.PlayerType = playerType;
        this.Score = 0; // Initialize score to zero
    }
}