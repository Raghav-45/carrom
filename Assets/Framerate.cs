using UnityEngine;

public class Framerate : MonoBehaviour
{
    private float fps;
    public TMPro.TextMeshProUGUI FPSCounterText;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(GetFPS), 1, 1);
    }

    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        FPSCounterText.text = "FPS: " + fps.ToString();
    }
}