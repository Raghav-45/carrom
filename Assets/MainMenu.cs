using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI MaxFPSText;
    int maxFrameRate = 60;

    private void Awake()
    {
        MaxFPSText.text = "Max FPS: " + Screen.currentResolution.refreshRateRatio.ToString();
        maxFrameRate = Screen.currentResolution.refreshRate;
    }

    public void PlayGame()
    {
        Application.targetFrameRate = maxFrameRate;
        SceneManager.LoadScene("Classic");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
