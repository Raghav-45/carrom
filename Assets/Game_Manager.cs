using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Game_Manager : MonoBehaviour
{
    public byte white = 0;
    public byte black = 0;
    [SerializeField] private Text whiteTextRenderer;
    [SerializeField] private Text blackTextRenderer;

    public Transform[] startPoints;
    public GameObject striker;
    public Transform PreviousPlayingCharacterResetPos;
    public Transform currentPlayingCharacterResetPos;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        striker = GameObject.FindGameObjectsWithTag("striker")[0];
        currentPlayingCharacterResetPos = startPoints[0];
    }

    public void SwitchToNextPlayer()
    {
        // Find the index of the current player
        int currentIndex = System.Array.IndexOf(startPoints, currentPlayingCharacterResetPos);

        // Update Reset Position to the previous player
        PreviousPlayingCharacterResetPos = startPoints[currentIndex];

        // Increment the index to switch to the next player
        currentIndex = (currentIndex + 1) % startPoints.Length;

        // Update Reset Position to the next player
        currentPlayingCharacterResetPos = startPoints[currentIndex];
    }

    public void giveTurn()
    {
        currentPlayingCharacterResetPos = PreviousPlayingCharacterResetPos;
    }

    public void UpdateScoreUI()
    {
        if (whiteTextRenderer != null && blackTextRenderer != null)
        {
            whiteTextRenderer.text = white.ToString();
            blackTextRenderer.text = black.ToString();
        }
    }

    public void restart()
    {
        // Get the current active scene and Reload it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}