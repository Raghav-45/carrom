using UnityEngine;

public class PawnDesignGenerator : MonoBehaviour
{
    [SerializeField] GameObject circlePrefab; // Reference to the GameObject you want to spawn

    // Coordinates of circle centers
    Vector2[] circleCoordinates = new Vector2[]
    {
        // new Vector2(0f, 0f), // Red
        // new Vector2(-0.1905256f, 0.11f), // White
        // new Vector2(0.1905256f, 0.11f), // White
        // new Vector2(0f, -0.22f), // White
        // new Vector2(0f, -0.44f), // White
        // new Vector2(0.3810512f, 0.22f), // White
        // new Vector2(-0.3810512f, 0.22f), // White
        // new Vector2(-0.3810512f, -0.22f), // White
        // new Vector2(0f, 0.44f), // White
        // new Vector2(0.3810512f, -0.22f), // White
        new Vector2(0f, 0.22f), // Black
        new Vector2(0.1905256f, -0.11f), // Black
        new Vector2(-0.1905256f, -0.11f), // Black
        new Vector2(0.1905256f, -0.33f), // Black
        new Vector2(-0.1905256f, -0.33f), // Black
        new Vector2(0.1905256f, 0.33f), // Black
        new Vector2(0.3810512f, 0f), // Black
        new Vector2(-0.3810512f, 0f), // Black
        new Vector2(-0.1905256f, 0.33f) // Black
    };

    void Start()
    {
        SpawnCirclesAtCoordinates();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        // Draw a small sphere at each circle spawn point
        foreach (Vector2 position in circleCoordinates)
        {
            Gizmos.DrawSphere(new Vector3(position.x, position.y, 0), 0.1f);
        }
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