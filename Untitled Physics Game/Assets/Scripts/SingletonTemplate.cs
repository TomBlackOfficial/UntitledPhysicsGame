using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonTemplate<T> : MonoBehaviour where T : SingletonTemplate<T>
{
    public static T instance;

    protected void Awake()
    {
        if(instance == null)
        {
            instance = GetComponent<T>();
        }
        else
        {
            Destroy(instance.gameObject);
        }
    }
}
