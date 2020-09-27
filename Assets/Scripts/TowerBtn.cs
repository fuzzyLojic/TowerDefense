﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;       // needed to script text updates

public class TowerBtn : MonoBehaviour
{
    [SerializeField] private GameObject towerPrefab;

    [SerializeField] private Sprite sprite;

    [SerializeField] private int price;

    [SerializeField] private Text priceTxt;

    // allows other scripts to access this private variable
    public GameObject TowerPrefab{
        get{ return towerPrefab;}
    }

    public Sprite Sprite{ 
        get{ return sprite;}
    }

    public int Price{
        get{ return price;}
    }

    private void Start(){
        priceTxt.text = price + "$";
    }
}
