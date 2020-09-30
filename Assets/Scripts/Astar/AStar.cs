using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;   // needed for Math
using System.Linq;  // needed for OrderBy method for list

public static class AStar
{
    private static Dictionary<Point, Node> nodes;

    private static void CreateNodes(){
        nodes = new Dictionary<Point, Node>();

        // run through all the tiles in the game
        foreach(TileScript tile in LevelManager.Instance.Tiles.Values){
            // adds the node (tile) to the node dictionary
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static Stack<Node> GetPath(Point start, Point goal){
        if(nodes == null){
            CreateNodes();
        }

        // creates an open list to use with A* algorithm
        HashSet<Node> openList = new HashSet<Node>();
        // creates a closed list to use with A*
        HashSet<Node> closedList = new HashSet<Node>();

        // easy access path
        Stack<Node> finalPath = new Stack<Node>();


        Node currentNode = nodes[start];    // finds the start node
        openList.Add(currentNode);          // adds start node to openList

        while(openList.Count > 0){  // step 10.
            // 2 loops to move around position we are starting with to look at neighbors
            for(int x = -1; x <= 1; x++){
                for(int y = -1; y <= 1; y++){
                    Point neighbourPos = new Point(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                    // if tile is not outside the map and there isn't anything on it already, and it isn't the start tile
                    if(LevelManager.Instance.InBounds(neighbourPos) && LevelManager.Instance.Tiles[neighbourPos].Walkable && neighbourPos != currentNode.GridPosition){
                    
                        int gCost = 0;
                        if(Math.Abs(x - y) == 1){   // 1 move horz. or vert. results in == 1
                            gCost = 10;
                        }
                        else{
                            if(!ConnectedDiagonally(currentNode, nodes[neighbourPos])){
                                continue;   // this skips lines below and begins next iteration of for loop
                            }
                            gCost = 14; // diagonal moves will not == 1
                        }
                    
                        // 3. Adds the neighbor to the open list
                        Node neighbour = nodes[neighbourPos];

                        if(openList.Contains(neighbour)){
                            if(currentNode.G + gCost < neighbour.G){  // step 9.4
                                neighbour.CalcValues(currentNode, nodes[goal], gCost);  // step 4
                            }
                        }
                        else if(!closedList.Contains(neighbour)){    // step 9.1
                            openList.Add(neighbour);    // step 9.2
                            neighbour.CalcValues(currentNode, nodes[goal], gCost);
                        }
                    }
                }
            }

            // 5. & 8. Moves the current node from the open list to the closed list
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            // 7.
            if(openList.Count > 0){
                // sorts list by F value and selects first or lowest value
                currentNode = openList.OrderBy(n => n.F).First();
            }

            if(currentNode == nodes[goal]){
                while(currentNode.GridPosition != start){
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;
                }
                break;
            }
        }

        return finalPath;

        //*** THIS IS FOR DEBUGGING, NEEDS TO BE REMOVED LATER ***//
        // GameObject.Find("AStarDebugger").GetComponent<AStarDebugger>().DebugPath(openList, closedList, finalPath);
        // Debug.Log("finished GetPath");
    }

    // this function is to prevent corner cutting
    private static bool ConnectedDiagonally(Node currentNode, Node neighbor){
        Point direction = neighbor.GridPosition - currentNode.GridPosition;

        Point first = new Point(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Point second = new Point(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);
        // if the node is outside the grid or is not walkable
        if(LevelManager.Instance.InBounds(first) && !LevelManager.Instance.Tiles[first].Walkable){
            return false;
        }
        if(LevelManager.Instance.InBounds(second) && !LevelManager.Instance.Tiles[second].Walkable){
            return false;
        }

        return true;
    }
}
