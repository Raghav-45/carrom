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

    // Update is called every frame, if the MonoBehaviour is enabled
    private void Update()
    {
        if (currentPlayingCharacter.GetComponent<Rigidbody2D>().velocity.magnitude <= currentPlayingCharacter.GetComponent<striker>().resetThresholdVelocity)
        {
            // ReturnStriker(); // Reset Striker Position
            currentPlayingCharacter.GetComponent<striker>().MoveStriker(false);
            if (currentPlayingCharacter.GetComponent<striker>().movestriker == false)
            {
                currentPlayingCharacter.GetComponent<striker>().breakshots[3].Stop();
                currentPlayingCharacter.GetComponent<striker>().control();
            }
        }
    }

    public void restart()
    {
        // Get the current active scene and Reload it
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}