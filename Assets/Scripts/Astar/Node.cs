using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;   // needed for Math

public class Node
{
    public Point GridPosition { get; private set;}

    public TileScript TileRef { get; private set;}

    public Vector2 WorldPosition { get; set; }

    public Node Parent { get; private set; }

    public int G { get; set; }

    public int H { get; set; }

    public int F { get; set; }

    public Node(TileScript tileRef){
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.WorldPosition = tileRef.WorldPosition;
    }

    // called in AStar.cs GetPath()
    public void CalcValues(Node parent, Node goal, int gCost){
        this.Parent = parent;
        this.G = gCost + parent.G;
        this.H = (Math.Abs(GridPosition.X - goal.GridPosition.X) + Math.Abs(GridPosition.Y - goal.GridPosition.Y)) * 10;
        this.F = G + H;
    }
}
