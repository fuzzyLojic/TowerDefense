using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    // this is a temp object for testing
    // [SerializeField] private GameObject towerPrefab;

    // public GameObject TowerPrefab{
    //     get{ return towerPrefab;}
    // }

    // access prefab from another script
    public TowerBtn ClickedBtn { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickTower(TowerBtn towerBtn){
        this.ClickedBtn = towerBtn;
    }

    public void BuyTower(){
        ClickedBtn = null;      // forces new tower to be chosen after each is placed
    }
}
