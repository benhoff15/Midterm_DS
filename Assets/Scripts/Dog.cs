using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Vector2 jumpPosition;

    Animator anim;
    bool isDead;
    Vector2 upPos;
    float delay = 0.5f;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Vector2.Distance(transform.position, jumpPosition) > 0 && !isDead)
        {
            transform.position = Vector2.MoveTowards(transform.position, jumpPosition, 3 * Time.deltaTime);
        }
        else
        {
            anim.SetTrigger("Jump");
            if (!isDead)
            {
                isDead = true;
                Destroy(gameObject, 1f);
                upPos = (Vector2)transform.position + Vector2.up;
                GameManager.Instance.CallCreateDucks();
            }
            if (delay <= 0)
            {
                transform.position = Vector2.Lerp(transform.position, upPos, 10 * Time.deltaTime);
            }
            else
            {
                delay -= Time.deltaTime;
            }
        }
    }
}