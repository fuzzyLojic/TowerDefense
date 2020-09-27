using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    // this is a temp object for testing
    // [SerializeField] private GameObject towerPrefab;

    // public GameObject TowerPrefab{
    //     get{ return towerPrefab;}
    // }

    // access prefab from another script
    public TowerBtn ClickedBtn { get; set; }

    private int currency;

    [SerializeField] private Text currencyTxt;

    public int Currency{
        get{ return currency;}

        set{
            this.currency = value;
            this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Currency = 5;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerBtn towerBtn){
        if(Currency >= towerBtn.Price){
            // stores the clicked button
            this.ClickedBtn = towerBtn;

            // activates hover icon
            Hover.Instance.Activate(towerBtn.Sprite);
        }

        
    }

    public void BuyTower(){
        if(Currency >= ClickedBtn.Price){
            Currency -= ClickedBtn.Price;
            Hover.Instance.Deactivate();
        }
    }

    // handles many uses of the Escape key
    private void HandleEscape(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Hover.Instance.Deactivate();
        }
    }
}
