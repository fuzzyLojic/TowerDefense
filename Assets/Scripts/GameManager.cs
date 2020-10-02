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

    public ObjectPool Pool { get; set; }

    private int currency;

    private int wave = 0;

    [SerializeField] private Text waveText;

    [SerializeField] private Text currencyTxt;

    [SerializeField] private GameObject waveBtn;

    List<Monster> activeMonsters = new List<Monster>();

    public int Currency{
        get{ return currency;}

        set{
            this.currency = value;
            this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";
        }
    }

    public bool WaveActive{
        get{ return activeMonsters.Count > 0;}
    }


    private void Awake() {
        Pool = GetComponent<ObjectPool>();
    }

    // Start is called before the first frame update
    void Start()
    {
        Currency = 100;
    }

    // Update is called once per frame
    void Update()
    {
        HandleEscape();
    }

    public void PickTower(TowerBtn towerBtn){
        if(Currency >= towerBtn.Price && !WaveActive){
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

    public void StartWave(){
        wave++;
        waveText.text = string.Format("Wave: <color=lime>{0}</color>", wave);
        StartCoroutine(SpawnWave());
        waveBtn.SetActive(false);
        
    }

    private IEnumerator SpawnWave(){
        LevelManager.Instance.GeneratePath();
        for (int i = 0; i < wave; i++){
            int monsterIndex = Random.Range(0, 4);

            // for debugging
            // monsterIndex = 3;

            string type = string.Empty;

            switch (monsterIndex)
            {
                case 0:
                    type = "BlueMonster";
                    break;
                case 1:
                    type = "RedMonster";
                    break;
                case 2:
                    type = "GreenMonster";
                    break;
                case 3:
                    type = "PurpleMonster";
                    break;
            }

            Monster monster = Pool.GetObject(type).GetComponent<Monster>();
            monster.Spawn();
            activeMonsters.Add(monster);

            yield return new WaitForSeconds(2.5f);            
        }
    }

    // called from Monster.cs
    public void RemoveMonster(Monster monster){
        activeMonsters.Remove(monster);

        if(!WaveActive){
            waveBtn.SetActive(true);
        }
    }
}
