using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour {

    public GameObject flecha, lista;

    int indice = 0;

	// Use this for initialization
	void Start () {
        Dibujar();
	}
	
	// Update is called once per frame
	void Update () {
        /*float vertical = Input.GetAxis("Vertical");
        if (vertical > 0) indice--;
        if (vertical < 0) indice++;
        if (indice >= 3) indice = 0;
        if (vertical != 0) Dibujar();*/
        bool up = Input.GetKeyDown("up");
        bool down = Input.GetKeyDown("down");
        
        if (up) indice--;
        if (down) indice++;

        if (indice > lista.transform.childCount - 1) indice = 0;
        else if (indice < 0) indice = lista.transform.childCount - 1;

        if (up || down) Dibujar();

        if (Input.GetKeyDown("return"))Accion();
	}

    void Dibujar() {
        Transform opcion = lista.transform.GetChild(indice);
        flecha.transform.position = opcion.position;
    }
    void Accion() {
        Transform opcion = lista.transform.GetChild(indice);

        if (opcion.gameObject.name == "Exit") Application.Quit();
        else SceneManager.LoadScene(opcion.gameObject.name);
    }
}
