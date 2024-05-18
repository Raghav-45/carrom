using UnityEngine;
using System.Collections.Generic;

public class PawnDesignGenerator : MonoBehaviour
{
    // [SerializeField] GameObject circlePrefab; // Reference to the GameObject you want to spawn

    [SerializeField] GameObject redPrefab;
    [SerializeField] GameObject whitePrefab;
    [SerializeField] GameObject blackPrefab;
    GameObject coinObject;

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
        // this.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(-9f, 18f));
        GameManager.Instance.SpawnCoin += SpawnAt;

        Debug.Log(transform.childCount);
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

    private void DrawDebugWireSphere(Vector2 center, float radius, float duration)
    {
        const int segments = 36; // Number of segments to approximate the circle
        float segmentAngle = 360f / segments;

        Vector2 startPoint = Vector2.zero;
        Vector2 prevPoint = Vector2.zero;

        for (int i = 0; i <= segments; i++)
        {
            float angle = i * segmentAngle;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * radius;
            Vector2 point = center + new Vector2(x, y);

            if (i > 0)
            {
                Debug.DrawLine(prevPoint, point, Color.red, duration); // Draw line between previous and current point with duration
            }

            prevPoint = point;

            if (i == 0)
            {
                startPoint = point;
            }
            else if (i == segments)
            {
                Debug.DrawLine(point, startPoint, Color.red, duration); // Connect last point to the start point with duration
            }
        }
    }

    private Vector2 FindClearLocation()
    {
        Vector2 clearLocation = Vector2.zero;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (!child.GetComponent<ColliderCheck>().isColliderTriggered)
            {
                clearLocation = child.transform.position;
                break; // Stop loop once a clear location is found
            }
        }

        return clearLocation;
    }

    void SpawnAt(CoinType coinType, Vector2 location)
    {
        List<Vector2> spawnLocations = new List<Vector2>();
        Vector2 clearLocation = FindClearLocation();

        Debug.Log(clearLocation);

        GameObject coinPrefab = null;

        // Select the appropriate prefab based on the coin type
        switch (coinType)
        {
            case CoinType.Red:
                coinPrefab = redPrefab;
                break;
            case CoinType.Black:
                coinPrefab = blackPrefab;
                break;
            case CoinType.White:
                coinPrefab = whitePrefab;
                break;
        }

        // Instantiate the coin prefab at the specified location with no rotation
        coinObject = Instantiate(coinPrefab, clearLocation, Quaternion.identity);

        // Set the scale of the instantiated coin object
        coinObject.transform.localScale = new Vector3(0.56f, 0.56f, 0.56f);

        // Optionally, you can set a parent for the spawned circles
        coinObject.transform.parent = transform;
    }

    // void SpawnCirclesAtCoordinates()
    // {
    //     // Loop through each coordinate and spawn a circle GameObject
    //     foreach (var kvp in coordinateColors)
    //     {
    //         Vector2 position = kvp.Key;
    //         Color color = kvp.Value;

    //         GameObject circle = Instantiate(circlePrefab, position, Quaternion.identity);
    //         // Optionally, you can set a parent for the spawned circles
    //         circle.transform.parent = transform;
    //     }
    // }
}