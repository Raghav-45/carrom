using UnityEngine;
using System.Collections.Generic;

public class PawnDesignGenerator : MonoBehaviour
{
    [SerializeField] GameObject circlePrefab; // Reference to the GameObject you want to spawn

    // Dictionary to map coordinates to colors
    Dictionary<Vector2, Color> coordinateColors = new Dictionary<Vector2, Color>()
    {
        { new Vector2(0f, 0f), Color.red }, // Red

        { new Vector2(-0.1905256f, 0.11f), Color.white }, // White
        { new Vector2(0.1905256f, 0.11f), Color.white },  // White
        { new Vector2(0f, -0.22f), Color.white }, // White
        { new Vector2(0f, -0.44f), Color.white }, // White
        { new Vector2(0.3810512f, 0.22f), Color.white }, // White
        { new Vector2(-0.3810512f, 0.22f), Color.white }, // White
        { new Vector2(-0.3810512f, -0.22f), Color.white }, // White
        { new Vector2(0f, 0.44f), Color.white }, // White
        { new Vector2(0.3810512f, -0.22f), Color.white }, // White

        { new Vector2(0f, 0.22f), Color.black }, // Black
        { new Vector2(0.1905256f, -0.11f), Color.black }, // Black
        { new Vector2(-0.1905256f, -0.11f), Color.black }, // Black
        { new Vector2(0.1905256f, -0.33f), Color.black }, // Black
        { new Vector2(-0.1905256f, -0.33f), Color.black }, // Black
        { new Vector2(0.1905256f, 0.33f), Color.black }, // Black
        { new Vector2(0.3810512f, 0f), Color.black }, // Black
        { new Vector2(-0.3810512f, 0f), Color.black }, // Black
        { new Vector2(-0.1905256f, 0.33f), Color.black } // Black
    };

    void Start()
    {
        // SpawnCirclesAtCoordinates();
        this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-9f, 18f));
    }

    // void OnDrawGizmos()
    // {
    //     foreach (var kvp in coordinateColors)
    //     {
    //         Vector2 position = kvp.Key;
    //         Color color = kvp.Value;

    //         Gizmos.color = color;
    //         Gizmos.DrawSphere(new Vector3(position.x, position.y, 0), 0.1f);
    //     }
    // }

    void SpawnCirclesAtCoordinates()
    {
        // Loop through each coordinate and spawn a circle GameObject
        foreach (var kvp in coordinateColors)
        {
            Vector2 position = kvp.Key;
            Color color = kvp.Value;

            GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
            // Optionally, you can set a parent for the spawned circles
            circle.transform.parent = transform;
        }
    }
}