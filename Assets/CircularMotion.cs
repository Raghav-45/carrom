using UnityEngine;

public class CircularMotion : MonoBehaviour
{
    public float speed = 75f;
    private float angle = 0f;

    private void Update()
    {
        if (gameObject.active)
        {
            angle += speed * Time.deltaTime;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}