using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;      // used in PlaceTower function

public class TileScript : MonoBehaviour
{
    public Point GridPosition {get; private set;}   // this is a property type variable

    public bool IsEmpty { get; private set;}

    private Color32 fullColor = new Color32(255, 118, 118, 255); // red
    private Color32 emptyColor = new Color32(96, 255, 90, 255); // green
    private SpriteRenderer spriteRenderer;

    // public SpriteRenderer SpriteRenderer { get; set; }

    public bool Walkable { get; set; }      // for A* pathfinding algorithm

    public bool Debugging { get; set; }     // for debugging A*

    public Vector2 WorldPosition{
        get{return new Vector2(transform.position.x + (GetComponent<SpriteRenderer>().bounds.size.x/2), transform.position.y - (GetComponent<SpriteRenderer>().bounds.size.y/2));}
    }

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
    // called from LevelManager
    public void Setup(Point gridPos, Vector3 worldPos, Transform parent){
        Walkable = true;    // for A* pathfinding algorithm
        IsEmpty = true;     // for tower placement
        this.GridPosition = gridPos;    // level floor as grid
        transform.position = worldPos;  // level floor as screen 
        transform.SetParent(parent);
        // add to Dictionary
        // this "LevelManager" is returned via the abstract Singleton class
        // this makes for easy access from other scripts
        LevelManager.Instance.Tiles.Add(gridPos, this);
        // Tiles.Add(gridPos, this);
    }

    private void OnMouseOver(){
        // if statement checks if mouse if over NOT a button to create the tower
        if(!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null){
            
            if(IsEmpty && !Debugging){
                ColorTile(emptyColor);
            }

            if(!IsEmpty && !Debugging){
                ColorTile(fullColor);
            }
            else if(Input.GetMouseButtonDown(0)){
                PlaceTower();
            }
        }
    }

    private void OnMouseExit(){
        if(!Debugging){
            ColorTile(Color.white); // return original tile color
        }
    }

    private void PlaceTower(){        
        GameObject tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
        // next line ensures that towers toward the bottom are rendered in front of higher towers
        tower.GetComponent<SpriteRenderer>().sortingOrder = GridPosition.Y;
        tower.transform.SetParent(transform);
        IsEmpty = false;
        ColorTile(Color.white);
        GameManager.Instance.BuyTower(); 
        Walkable = false;   // for A* pathfinding algorithm       
    }

    private void ColorTile(Color newColor){
        spriteRenderer.color = newColor;
    }
}
