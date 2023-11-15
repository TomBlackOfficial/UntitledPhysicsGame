using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private List<Collider2D> collidersToIgnore = new List<Collider2D>();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collidersToIgnore.Contains(collision.collider))
        {
            return;
        }

        player.Die();
    }
}
