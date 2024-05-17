using System;
using System.Collections.Generic;
using System.Linq;
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
    public event Action<CoinType, Vector2> SpawnCoin;

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
        players[currentPlayerIndex].currentTurnCoinStack.resetCoinStack();

        players[currentPlayerIndex].isQueenCoveringMove = false; // Reset this flag for Previous Player
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Length;

        players[currentPlayerIndex].isPlayerTurn = true;
        players[currentPlayerIndex].currentTurnCoinStack.resetCoinStack();

        // TODO: Do this only for Classic Mode
        if (GameObject.FindGameObjectsWithTag("red").Length < 1)
        {
            bool shouldSpawn = true;

            foreach (Player player in players)
            {
                if (player.coinStack.GetCoinCount(CoinType.Red) == 1)
                {
                    shouldSpawn = false;
                    break; // Exit the loop after spawning one red coin
                }
            }

            if (shouldSpawn)
            {
                // SpawnCoinOnBoard(CoinType.Red);
                SpawnCoin?.Invoke(CoinType.Red, new Vector2(0f, 0f));
            }
        }

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
        // FindClearLocation();
        Player currentPlayer = GetCurrentPlayer();
        currentPlayer.CollectCoin(type);
        OnCoinCollected?.Invoke(currentPlayer, type); // Coin Collect Event
    }

    public void onResetPlayerLocation()
    {
        Player currentPlayer = GetCurrentPlayer();
        currentPlayer.currentTurnCoinStack = new CoinStack(); // Initialize the coin stack for the current turn
    }

    public void OnFoul()
    {
        OnStrikerFoul?.Invoke();
    }

    public void SpawnCoinOnBoard(CoinType coinType)
    {
        SpawnCoin?.Invoke(coinType, new Vector2(0f, 0f));
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
    public PlayerTurn PlayerType;
    public Transform startPoint;
    public int score; // Player's score
    public CoinStack coinStack;
    public CoinStack currentTurnCoinStack;

    // [Header("Coins")]
    // public int redCoin; // Number of red coins collected
    // public int blackCoin; // Number of black coins collected
    // public int whiteCoin; // Number of white coins collected

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
        this.isPlayerTurn = isPlayerTurn;
        isQueenCoveringMove = false;
        this.StrikerImage = StrikerImage;
        this.coinStack = new CoinStack(); // Initialize the overall coin stack
        this.currentTurnCoinStack = new CoinStack(); // Initialize the coin stack for the current turn

    }

    public void UpdateCoinTextsUI()
    {
        // TODO: If the velocity of the player's striker is below threshold velocity, then stop all coins to ensure no errors occur.

        redCoinText.text = coinStack.GetCoinCount(CoinType.Red).ToString();
        blackCoinText.text = coinStack.GetCoinCount(CoinType.Black).ToString();
        whiteCoinText.text = coinStack.GetCoinCount(CoinType.White).ToString();
    }

    // Method to handle coin functionality based on coin type
    public void CollectCoin(CoinType type)
    {
        currentTurnCoinStack.resetCoinStack(); // Reset currentTurnCoinStack to ensure correct results
        switch (type)
        {
            case CoinType.Red:
                isQueenCoveringMove = true;
                break;

            case CoinType.Black:
            case CoinType.White:
                coinStack.AddCoin(type); // Add the coin to the CoinStack
                currentTurnCoinStack.AddCoin(type); // Add the coin to the currentTurnCoinStack
                if (isQueenCoveringMove)
                {
                    coinStack.AddCoin(CoinType.Red); // Add red coin if Queen Covering Move
                    currentTurnCoinStack.AddCoin(CoinType.Red); // Add red coin if Queen Covering Move
                    isQueenCoveringMove = false;
                }
                break;

        }
        //score = coinStack.CoinsCount();
        score = (coinStack.GetCoinCount(CoinType.Black) * 10) + (coinStack.GetCoinCount(CoinType.White) * 20) + (coinStack.GetCoinCount(CoinType.Red) * 50);
        // currentTurnCoinStack = coinStack;
        UpdateCoinTextsUI();
    }

    public void OnFoul()
    {
        GameManager.Instance.OnFoul();
        if (currentTurnCoinStack.CoinsCount() > 0)
        {
            coinStack.RemoveLastCoin();
        }
        else if (coinStack.CoinsCount() > 0)
        {
            if (coinStack.GetCoinCount(CoinType.Black) > 0)
            {
                // blackCoin--;
                coinStack.RemoveCoin(CoinType.Black);
            }
            else if (coinStack.GetCoinCount(CoinType.White) > 0)
            {
                // whiteCoin--;
                coinStack.RemoveCoin(CoinType.White);
            }
            else if (coinStack.GetCoinCount(CoinType.Red) > 0)
            {
                // redCoin = 0;
                coinStack.RemoveCoin(CoinType.Red);
            }
        }
        UpdateCoinTextsUI();
    }
}

[System.Serializable]
public class CoinStack
{
    public List<CoinType> coins;

    public CoinStack()
    {
        coins = new List<CoinType>();
    }

    public void resetCoinStack()
    {
        coins = new List<CoinType>();
    }

    public int CoinsCount()
    {
        return coins.Count;
    }

    // Add a coin to the stack
    public void AddCoin(CoinType coinType)
    {
        coins.Add(coinType);
    }

    // Remove the last coin from the stack
    public void RemoveLastCoin()
    {
        if (coins.Count > 0)
        {
            GameManager.Instance.SpawnCoinOnBoard(GetLastCoin());
            coins.RemoveAt(coins.Count - 1);
        }
    }

    public CoinType GetLastCoin()
    {
        return coins[coins.Count - 1];
    }

    // Remove a specific coin type from the stack
    public void RemoveCoin(CoinType coinType)
    {
        if (coins.Contains(coinType))
        {
            GameManager.Instance.SpawnCoinOnBoard(coinType);
            coins.Remove(coinType);
        }
    }

    // Count of a specific coin type from the stack
    public int GetCoinCount(CoinType coinType)
    {
        return coins.Count(c => c == coinType);
    }
}