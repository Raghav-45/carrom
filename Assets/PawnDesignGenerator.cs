using UnityEngine;

public class PawnDesignGenerator : MonoBehaviour
{
    public GameObject cornerPrefab; // Prefab for corners
    public GameObject sideCenterPrefab; // Prefab for side centers

    void Start()
    {
        // Generate the first hexagon with corners
        GenerateHexagonCorners(Vector3.zero, 5f); // Adjust the size as needed

        // Generate the second hexagon with side centers
        GenerateHexagonSideCenters(new Vector3(8f, 0f, 0f), 4f); // Adjust the position and size as needed
    }

    // Function to generate corners of a hexagon
    void GenerateHexagonCorners(Vector3 center, float size)
    {
        for (int i = 0; i < 6; i++)
        {
            float angle = i * 60 * Mathf.Deg2Rad;
            Vector3 cornerPosition = center + new Vector3(size * Mathf.Cos(angle), 0f, size * Mathf.Sin(angle));
            Instantiate(cornerPrefab, cornerPosition, Quaternion.identity);
        }
    }

    // Function to generate centers of sides of a hexagon
    void GenerateHexagonSideCenters(Vector3 center, float size)
    {
        for (int i = 0; i < 6; i++)
        {
            float angle1 = i * 60 * Mathf.Deg2Rad;
            float angle2 = (i + 1) * 60 * Mathf.Deg2Rad;
            Vector3 sideCenterPosition = center + new Vector3((size * Mathf.Cos(angle1) + size * Mathf.Cos(angle2)) / 2f,
                                                               0f,
                                                               (size * Mathf.Sin(angle1) + size * Mathf.Sin(angle2)) / 2f);
            Instantiate(sideCenterPrefab, sideCenterPosition, Quaternion.identity);
        }
    }
}