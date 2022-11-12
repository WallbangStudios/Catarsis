using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public List<Item> Items = new List<Item>();

    public int Width;
    public int Height;

    public GameObject SlotsPrefab;

    public GameObject[,] Slots;

    public InvManager Manager;

    //Actualizar la lista de Items
    public static void RefreshItems(Inventory Inventory) 
    {
        //Se borra todo
        Inventory.Items.Clear();
        //Recorre cada Slot y si tiene un object, entonces lo agregara a la lista
        for (int y = 0; y < Inventory.Height; y++)
        {
            for (int x = 0; x < Inventory.Width; x++)
            {
                slot Analize = Inventory.Slots[x, y].GetComponent<slot>();
                //print(Inventory.Slots[x, y].transform.name);
                if (Analize.Item != null)
                    Inventory.Items.Add(Analize.Localization);
            }
        }
    }


    // Use this for initialization
    void Start()
    {

        //Contructor de los Slots
        Slots = new GameObject[Width, Height];
        //Creara todos los slots y le asigna a cada uno una posicion Cartesiana
        for (int y = 0; y < Height; y++) {
            for (int x = 0; x < Width; x++) {
                GameObject obj = (GameObject)Instantiate(SlotsPrefab);
                obj.transform.SetParent(this.transform);
                obj.transform.name = "(" + x + ", " + y + ")";
                obj.GetComponent<slot>().Localization = new Item(x, y, 0, 0, "");
                Slots[x, y] = obj.gameObject;
                obj.GetComponent<RectTransform>().localScale = Vector3.one;
                for (int i = 0; i < Items.Count; i++) {
                    if (Items[i].X == x && Items[i].Y == y) {
                        GameObject p = Manager.SearchItemByName(Items[i].Name);
                        GameObject a = (GameObject)Instantiate(p);
                        a.transform.name = Items[i].Name;
                        a.transform.SetParent(Slots[x, y].transform);
                        a.GetComponent<RectTransform>().localScale = Vector3.one;
                    }
                    
                }
            }
        }	
	}

    // Update is called once per frame
    void Update()
    {
        
	}
}

[System.Serializable]
public class Item
{
    //Todo item tendrá:
    //Un Nombre del item
    public string Name;
    //una posicion cartesiana en los slots
    public int X;
    public int Y;

    //y un tamaño
    public int Width;
    public int Height;
    
    //Constructor
    public Item(int x, int y, int width, int height, string name)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
        Name = name;
    }
    //Constructor vacio
    public Item()
    {
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
        Name = "";
    }
}
