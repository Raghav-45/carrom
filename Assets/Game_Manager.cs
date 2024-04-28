using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game_Manager : MonoBehaviour
{
    public byte white = 0;
    public byte black = 0;
    [SerializeField] private Text whiteTextRenderer;
    [SerializeField] private Text blackTextRenderer;
    public GameObject[] strikers;
    public GameObject currentPlayingCharacter;

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        strikers = GameObject.FindGameObjectsWithTag("striker");
    }

    // Start is called before the first frame update
    void Start()
    {
        currentPlayingCharacter = strikers[0];
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