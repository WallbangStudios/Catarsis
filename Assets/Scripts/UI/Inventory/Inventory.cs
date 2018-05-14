using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public Texture2D Image;
    public Rect Position;

    public Color fillColor;

    public GameObject SlotPrefab;

    public GameObject[,] Cells;

    public int Width = 11;
    public int Height = 7;
    
	// Use this for initialization
/*
    void Awake () {
        Cells = new GameObject[Width, Height];
        for (int y = 0; y < Height; y++){
            for (int x = 0; x < Width; x++){
                GameObject obj = (GameObject)Instantiate(SlotPrefab);

                obj.transform.SetParent(this.transform);
                obj.transform.GetComponent<CellsScript>().item = new Item(x, y, 0, 0);
                obj.transform.name = "(" + x + ", " + y + ")";
                Cells[x, y] = obj;

            }
        }
	}
    */
    // Update is called once per frame
    void Update()
    {
		
	}

    void OnGUI()
    {
        GUI.DrawTexture(Position, Image);
    }

}

public class Item {
    public int X;
    public int Y;

    public int Width;
    public int Height;

    public Item (int x,int y, int width, int height){
        X=x;
        Y=y;
        Width=width;
        Height=height;
    }
    public Item() {
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
    }
}

public class Weapon : Item{

    public DistanceAttack DA;

}
