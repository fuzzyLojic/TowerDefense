using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] private GameObject[] tilePrefabs;

    [SerializeField] private CameraMovement cameraMovement;

    [SerializeField] private Transform map;

    private Point blueSpawn;
    private Point redSpawn;
    [SerializeField] private GameObject bluePortalPrefab;
    [SerializeField] private GameObject redPortalPrefab;

    public Portal BluePortal { get; set; }

    private Point mapSize;

    private Stack<Node> path;

    public Stack<Node> Path{
        get{
            if(path == null){
                GeneratePath();
            }
            // duplicates of stack created for each monster so later monsters don't have partially popped stacks
            return new Stack<Node>(new Stack<Node>(path));
        }
    }

    public Dictionary<Point, TileScript> Tiles {get; set;}

    public float TileSize{
        get{ return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;}
    }

    // Start is called before the first frame update
    void Start()
    {      
        // TestDictionary();

        // int a = 1;   // example use of singlton (Generics)
        // int b = 2;
        // Debug.Log("A: " + a + "\nB: " + b);
        // Swap<int>(ref a, ref b);
        // Debug.Log("A: " + a + "\nB: " + b);

        // string aString = "A";
        // string bString = "B";
        // Debug.Log("aString: " + aString + "\nbString: " + bString);
        // Swap<string>(ref aString, ref bString);
        // Debug.Log("aString: " + aString + "\nbString: " + bString);


        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // swap function using generics
    public void Swap<T>(ref T a, ref T b){
        T tmp = a;
        a = b;
        b = tmp;
    }

    // private void TestDictionary(){
    //     Dictionary<string, int> testDictionary = new Dictionary<string, int>();
    //     testDictionary.Add("Age", 28);
    //     testDictionary.Add("Strength", 300);
    //     testDictionary.Add("Farts", 999);

    //     Debug.Log("My farts is " + testDictionary["Farts"]);
    // }


    private void CreateLevel(){
        Tiles = new Dictionary<Point, TileScript>();

        string[] mapData = ReadLevelText();     // to read level map grid from text file

        mapSize = new Point(mapData[0].ToCharArray().Length, mapData.Length);   // x value of map, y value of map

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero; // don't want unassigned variable

        // get starting coordinates (top left) for beginning of tile placement
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for(int y = 0; y < mapY; y++){
            char[] newTiles = mapData[y].ToCharArray();
            for(int x = 0; x < mapX; x++){
                // place the tile in the world
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        // get last tile for camera limits
        maxTile = Tiles[new Point(mapX - 1, mapY - 1)].transform.position;

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));

        SpawnPortals();
    }

    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart){
        // "1" = 1
        int tileIndex = int.Parse(tileType);
        // creates new tile
        TileScript newTile = Instantiate(tilePrefabs[tileIndex]).GetComponent<TileScript>();

        // uses the new tile variable to change the position of the tile (before TileScript)
        // newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);

        newTile.Setup(new Point(x, y), new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0), map);


        // temporary return for getting max x, y position for world
        // return newTile.transform.position;
    }

    private string[] ReadLevelText(){
        // load text document "Level" from Assets folder "Resources"
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        // remove newlines - "Environment" requires "using System;" above
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }


    private void SpawnPortals(){
        blueSpawn = new Point(0, 0);
        GameObject tmp = Instantiate(bluePortalPrefab, Tiles[blueSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);
        BluePortal = tmp.GetComponent<Portal>();
        BluePortal.name = "BluePortal";

        redSpawn = new Point(11, 6);
        Instantiate(redPortalPrefab, Tiles[redSpawn].GetComponent<TileScript>().WorldPosition, Quaternion.identity);

    }

    // test if tile is in-bounds of map or if trying to read something that doesn't exist
    public bool InBounds(Point position){
        return position.X >= 0 && position.Y >= 0 && position.X < mapSize.X && position.Y < mapSize.Y;
    }

    // called from GameManager
    public void GeneratePath(){
        path = AStar.GetPath(blueSpawn, redSpawn);
    }

}
