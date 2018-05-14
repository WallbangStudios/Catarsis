using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellsScript : MonoBehaviour {

    public Item item;
    public Text txt;

    public GameObject StoredItemObject;
    public bool IsOccupied;

	// Use this for initialization
	void Start () {
        txt.text = "(" + item.X + ", " + item.Y + ")";
	}

}
