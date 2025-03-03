using UnityEngine;

public class Log : MonoBehaviour
{
    public float speed = 2f; // Adjust speed as needed

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
