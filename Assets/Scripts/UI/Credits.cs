using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits : MonoBehaviour {

    public GameObject Creditos;

    public Text[] textos;

    int indice = 0;

	// Use this for initialization
	void Start () {

        foreach(Text elemento in textos){
            elemento.enabled = false;
        }
        textos[indice].enabled = true;
		
	}
	
	// Update is called once per frame
	void Update () {
        bool up = Input.GetKeyDown("up");
        bool right = Input.GetKeyDown("right");
        
        bool down = Input.GetKeyDown("down");
        bool left = Input.GetKeyDown("left");

        if (up || right)
        {
            textos[indice].enabled = false;
            indice++;
        }
        if (down || left) {
            textos[indice].enabled = false;
            indice--;
        }

        if (indice > Creditos.transform.childCount - 1) indice = 0;
        else if (indice < 0) indice = Creditos.transform.childCount - 1;

        if (up || left || down || right) {
            textos[indice].enabled = true;
        }
	}

}
