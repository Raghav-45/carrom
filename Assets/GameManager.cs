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
    [SerializeField] private int redCoins;
    [SerializeField] private int blackCoins;
    [SerializeField] private int whiteCoins;

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
}

public enum CoinType
{
    Red,
    Black,
    White
}