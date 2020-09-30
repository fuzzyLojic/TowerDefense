using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarDebugger : MonoBehaviour
{
    [SerializeField] private TileScript goal;
    [SerializeField] private TileScript start;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private GameObject debugTilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // comment in Update function to use AStarDebugger
    // void Update()
    // {
    //     ClickTile();

    //     if(Input.GetKeyDown(KeyCode.Space)){
    //         Debug.Log("space down");
    //         AStar.GetPath(start.GridPosition, goal.GridPosition);
    //     }
    // }

    private void ClickTile(){
        if(Input.GetMouseButtonDown(1)){    // right mouse button
            // create raycast from mouse to object under mouse
            // this method is used to avoid involving any other scripts since this is a debug only class
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if(hit.collider != null){
                // get reference to object clicked on
                TileScript tmp = hit.collider.GetComponent<TileScript>();

                if(tmp != null){    // if we didn't click on a tile, then there is no TileScript returned
                    if(start == null){  // first click makes start point
                        start = tmp;
                        // start.SpriteRenderer.color = new Color32(0, 255, 0, 255);
                        // start.Debugging = true;
                        CreateDebugTile(start.WorldPosition, new Color32(0, 255, 0, 255));
                    }
                    else if(goal == null){  // second click makes goal point
                        goal = tmp;
                        // goal.SpriteRenderer.color = new Color32(255, 0, 0, 255);
                        // goal.Debugging = true;
                        CreateDebugTile(goal.WorldPosition, new Color32(255, 0, 0, 255));
                    }
                }
            }
        }
    }

    public void DebugPath(HashSet<Node> openList, HashSet<Node> closedList, Stack<Node> path){
        foreach(Node node in openList){
            if(node.TileRef != start && node.TileRef != goal){
                // node.TileRef.SpriteRenderer.color = new Color32(0, 125, 125, 255);
                CreateDebugTile(node.TileRef.WorldPosition, new Color32(0, 125, 125, 255), node);
            }

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach(Node node in closedList){
            if(node.TileRef != start && node.TileRef != goal && !path.Contains(node)){
                CreateDebugTile(node.TileRef.WorldPosition, new Color32(125, 125, 0, 255), node);
            }

            PointToParent(node, node.TileRef.WorldPosition);
        }

        foreach(Node node in path){
            CreateDebugTile(node.TileRef.WorldPosition, new Color32(0, 0, 255, 255), node);
        }
    }

    private void PointToParent(Node node, Vector2 position){
        if(node.Parent != null){    // start position will not have a parent
            GameObject arrow = Instantiate(arrowPrefab, position, Quaternion.identity);
            arrow.GetComponent<SpriteRenderer>().sortingOrder = 3;

            // Right
            if((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 0);
            }
            // Top Right
            else if((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 45);
            }
            // Up
            else if((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 90);
            }
            // Top Left
            else if((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y > node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 135);
            }
            // Left
            else if((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y == node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 180);
            }
            // Bottom Left
            else if((node.GridPosition.X > node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 225);
            }
            // Bottom
            else if((node.GridPosition.X == node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 270);
            }
            // Up
            else if((node.GridPosition.X < node.Parent.GridPosition.X) && (node.GridPosition.Y < node.Parent.GridPosition.Y)){
                arrow.transform.eulerAngles = new Vector3(0, 0, 315);
            }
        }
        
    }


    private void CreateDebugTile(Vector3 worldPos, Color32 color, Node node = null){
        GameObject debugTile = Instantiate(debugTilePrefab, worldPos, Quaternion.identity);
        if(node != null){
            DebugTile tmp = debugTile.GetComponent<DebugTile>();
            tmp.G.text += node.G; // adds score to text element
            tmp.H.text += node.H; // adds score to text element
            tmp.F.text += node.F;
        }
        debugTile.GetComponent<SpriteRenderer>().color = color;
    }

}
