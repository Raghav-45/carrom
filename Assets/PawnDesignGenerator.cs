using UnityEngine;

public class PawnDesignGenerator : MonoBehaviour
{
    public int numberOfTiles = 30; // Number of tiles to generate around the circle
    public GameObject hexTilePrefab; // Prefab for the hexagonal tiles
    public GameObject queenPrefab; // Prefab for the queen piece
    public float circleRadius = 5f; // Radius of the circular board
    public float gapAngle = 360f / 30f; // Angle between each tile

    void Start()
    {
        GenerateBoard();
    }

    void GenerateBoard()
    {
        for (int i = 0; i < numberOfTiles; i++)
        {
            // Calculate angle for this tile
            float angle = i * gapAngle;

            // Convert angle to radians
            float radianAngle = angle * Mathf.Deg2Rad;

            // Calculate position of the tile using polar coordinates
            float x = circleRadius * Mathf.Cos(radianAngle);
            float z = circleRadius * Mathf.Sin(radianAngle);

            // Create hexagon tile prefab at the calculated position
            GameObject hexTile = Instantiate(hexTilePrefab, new Vector3(x, 0, z), Quaternion.identity);
            hexTile.transform.SetParent(transform);
        }

        // Instantiate queen prefab at the center
        GameObject queen = Instantiate(queenPrefab, Vector3.zero, Quaternion.identity);
        queen.transform.SetParent(transform);
        queen.name = "Queen";
    }
}