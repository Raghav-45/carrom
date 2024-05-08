using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI MaxFPSText;
    int maxFrameRate = 60;

    private void Awake()
    {
        Resolution[] resolutions = Screen.resolutions;

        MaxFPSText.text = "Max FPS: " + resolutions[resolutions.Length - 1].refreshRateRatio.ToString();

        maxFrameRate = resolutions[resolutions.Length - 1].refreshRate;
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
