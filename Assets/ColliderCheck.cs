using UnityEngine;

public class ColliderCheck : MonoBehaviour
{
    // Variable to indicate if the collider is triggered
    public bool isColliderTriggered = true;

    // OnTriggerEnter is called when the Collider other enters the trigger
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the other collider is the one you want to track
        if (other.CompareTag("white") || other.CompareTag("black") || other.CompareTag("red"))
        {
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
            // Set the flag to false
            isColliderTriggered = false;
        }
    }
}
