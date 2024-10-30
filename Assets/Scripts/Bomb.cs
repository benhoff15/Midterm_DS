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
        transform.Translate(Vector3.left * speed * Time.deltaTime);

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
        Duck[] ducks = FindObjectsOfType<Duck>();
        foreach (Duck duck in ducks)
        {
            duck.KillDuck();  
        }

        Destroy(gameObject);
    }
}