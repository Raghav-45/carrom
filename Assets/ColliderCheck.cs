using System;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    // Variable to indicate if the collider is triggered
    public bool isColliderTriggered = false;

    // Keep track of the number of colliders currently triggering
    private int triggeringColliders = 0;

    private void DrawDebugWireSphere(Vector2 center, float radius)
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
                Debug.DrawLine(prevPoint, point, Color.red); // Draw line between previous and current point with duration
            }

            prevPoint = point;

            if (i == 0)
            {
                startPoint = point;
            }
            else if (i == segments)
            {
                Debug.DrawLine(point, startPoint, Color.red); // Connect last point to the start point with duration
            }
        }
    }

    void Update()
    {
        if (!isColliderTriggered)
        {
            DrawDebugWireSphere(transform.position, (((0.22f) * 2) / 3) / (float)Math.Sqrt(2));
        }
    }

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is the one you want to track
        if (other.CompareTag("white") || other.CompareTag("black") || other.CompareTag("red"))
        {
            // Increment the count of triggering colliders
            triggeringColliders++;

            // Set the flag to true
            isColliderTriggered = true;
        }
    }

    // OnTriggerExit is called when the Collider other has stopped touching the trigger
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the other collider is the one you want to track
        if (other.CompareTag("white") || other.CompareTag("black") || other.CompareTag("red"))
        {
            // Decrement the count of triggering colliders
            triggeringColliders--;

            // If no colliders are currently triggering, set the flag to false
            if (triggeringColliders <= 0)
            {
                isColliderTriggered = false;
            }
        }
    }
}
