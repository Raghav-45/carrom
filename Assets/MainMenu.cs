using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        Application.targetFrameRate = 60;
        SceneManager.LoadScene("Classic");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
