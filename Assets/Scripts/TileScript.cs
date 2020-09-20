using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;      // used in PlaceTower function

public class TileScript : MonoBehaviour
{
    public Point GridPosition {get; private set;}   // this is a property type variable

    public Vector2 WorldPosition{
        get{return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));}
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    ///<summary>
    /// Sets up the tile, this is an alternative to a constructor
    ///</summary>
    ///<param name="gridPos">The tiles' grid postion</param>
    ///<param name="worldPos">The tiles' world position</param>
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent){
        this.GridPosition = gridPos;
        transform.position = worldPos;
        transform.SetParent(parent);
        // add to Dictionary
        // this "LevelManager" is returned via the abstract Singleton class
        // this makes for easy access from other scripts
        LevelManager.Instance.Tiles.Add(gridPos, this);
        // Tiles.Add(gridPos, this);
    }

    private void OnMouseOver(){
        if(Input.GetMouseButtonDown(0)){
            // if statement checks if mouse if over NOT a button to create the tower
            if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null){
                PlaceTower();
            }
        }
    }

    private void PlaceTower(){        
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        // next line ensures that towers toward the bottom are rendered in front of higher towers
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);
        GameManager.Instance.BuyTower();        
    }
}
