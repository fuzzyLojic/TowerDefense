using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;      // lowercase

    public static T Instance{       // uppercase
        get{
            if(instance == null){   // must be the lowercase one or stack overflow
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}
