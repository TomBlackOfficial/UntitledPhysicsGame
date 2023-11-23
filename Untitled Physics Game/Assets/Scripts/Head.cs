using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Collider2D> collidersToIgnore = new List<Collider2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IgnoreCollision(collision.collider))
        {
            return;
        }

        player.Die();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (IgnoreCollision(collider))
        {
            return;
        }

        player.Die();
    }

    public bool IgnoreCollision(Collider2D collider)
    {
        return collidersToIgnore.Contains(collider);
    }
}
