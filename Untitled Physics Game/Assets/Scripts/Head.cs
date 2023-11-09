using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private LayerMask collisionMask;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & collisionMask) != 0)
        {
            player.Die();
        }
    }
}
