using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;

    // allows other scripts to access this private variable
    public GameObject TowerPrefab{
        get{ return towerPrefab;}
    }

    
}
