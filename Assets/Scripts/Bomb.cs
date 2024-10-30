using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float speed = 10.0f;

    private void Start()
    {
        if (GetComponent<Collider2D>() == null)
        {
            gameObject.AddComponent<BoxCollider2D>();
        }
    }

    private void Update()
    {
        // Move leftward across the screen
        transform.Translate(Vector3.left * speed * Time.deltaTime);

        // Destroy if off the left edge of the screen
        if (transform.position.x < -10)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        Explode();
    }

    private void Explode()
    {
        // Trigger fall behavior in all ducks
        Duck[] ducks = FindObjectsOfType<Duck>();
        foreach (Duck duck in ducks)
        {
            duck.KillDuck();  // This will make them fall as when shot
        }

        // Destroy the bomb
        Destroy(gameObject);
    }
}