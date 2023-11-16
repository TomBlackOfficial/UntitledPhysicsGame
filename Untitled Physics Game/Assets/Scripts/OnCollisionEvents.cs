using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionEvents : MonoBehaviour
{
    public UnityEvent onCollisionEnter;
    public UnityEvent onCollisionExit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Head>())
        {
            onCollisionEnter.Invoke();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Head>())
        {
            onCollisionExit.Invoke();
        }
    }
}
