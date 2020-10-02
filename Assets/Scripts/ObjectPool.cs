using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject[] objectPrefabs;

    private List<GameObject> pooledObjects = new List<GameObject>();

    // called from Game Manager
    public GameObject GetObject(string type){
        // check for inactive object already in pool to use
        foreach (GameObject go in pooledObjects){
            if(go.name == type && !go.activeInHierarchy){   // object is in pool and not active
                go.SetActive(true);
                return go;
            }
        }

        // if no inactive object of type in pool, create new object
        for(int i = 0; i < objectPrefabs.Length; i++){
            if(objectPrefabs[i].name == type){
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }

        return null;
    }

    // deactivate to return to pool for later use
    public void ReleaseObject(GameObject gameObject){
        gameObject.SetActive(false);
    }
}
