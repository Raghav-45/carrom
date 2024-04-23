using UnityEngine;

public class PawnDesignGenerator : MonoBehaviour
{
    public GameObject circlePrefab; // Reference to the GameObject you want to spawn

    // Coordinates of circle centers
    private Vector2[] circleCoordinates = new Vector2[]
    {
        new Vector2(0.39f, 0.00f),
        new Vector2(0.20f, 0.34f),
        new Vector2(-0.20f, 0.34f),
        new Vector2(-0.39f, 0.00f),
        new Vector2(-0.20f, -0.34f),
        new Vector2(0.20f, -0.34f)
    };

    void Start()
    {
        SpawnCirclesAtCoordinates();
    }

    void SpawnCirclesAtCoordinates()
    {
        // Loop through each coordinate and spawn a circle GameObject
        for (int i = 0; i < circleCoordinates.Length; i++)
        {
            Vector2 position = circleCoordinates[i];
            GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
            // Optionally, you can set a parent for the spawned circles
            circle.transform.parent = transform;
        }
    }
}