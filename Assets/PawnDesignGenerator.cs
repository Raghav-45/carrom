using UnityEngine;
using System.Collections.Generic;

public class PawnDesignGenerator : MonoBehaviour
{
    // [SerializeField] GameObject circlePrefab; // Reference to the GameObject you want to spawn

    [SerializeField] GameObject redPrefab;
    [SerializeField] GameObject whitePrefab;
    [SerializeField] GameObject blackPrefab;

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

        for (int i = 0; i < transform.childCount; i++)
        {
            // Get the child object at index i
            Transform child = transform.GetChild(i);

            // Check if the child object is colliding with something
            if (child.GetComponent<Collider2D>().isTrigger && (child.CompareTag("white") || child.CompareTag("black") || child.CompareTag("red")))
            {
                // Do something if collision occurs
                Debug.Log("Collision detected with child object: " + child.name);
            }
            // else
            // {
            //     Debug.Log("Clean Area: " + child.transform.position);
            // }
        }
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

    void SpawnAt(CoinType coinType, Vector2 location)
    {
        // transform.GetChild(0);

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
        GameObject coinObject = Instantiate(coinPrefab, location, Quaternion.identity);

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