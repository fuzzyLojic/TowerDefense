using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tilePrefabs;

    [SerializeField] private CameraMovement cameraMovement;

    public float TileSize{
        get{ return tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;}
    }

    // Start is called before the first frame update
    void Start()
    {
        CreateLevel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateLevel(){
        string[] mapData = ReadLevelText();

        int mapX = mapData[0].ToCharArray().Length;
        int mapY = mapData.Length;

        Vector3 maxTile = Vector3.zero; // don't want unassigned variable

        // get starting coordinates (top left) for beginning of tile placement
        Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height));

        for(int y = 0; y < mapY; y++){
            char[] newTiles = mapData[y].ToCharArray();
            for(int x = 0; x < mapX; x++){
                // place the tile in the world
                maxTile = PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        cameraMovement.SetLimits(new Vector3(maxTile.x + TileSize, maxTile.y - TileSize));
    }

    private Vector3 PlaceTile(string tileType, int x, int y, Vector3 worldStart){
        // "1" = 1
        int tileIndex = int.Parse(tileType);
        // creates new tile
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);
        // uses the new tile variable to change the position of the tile
        newTile.transform.position = new Vector3(worldStart.x + (TileSize * x), worldStart.y - (TileSize * y), 0);

        // temporary return for getting max x, y position for world
        return newTile.transform.position;
    }

    private string[] ReadLevelText(){
        // load text document "Level" from Assets folder "Resources"
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        // remove newlines
        string data = bindData.text.Replace(Environment.NewLine, string.Empty);
        return data.Split('-');
    }
}
