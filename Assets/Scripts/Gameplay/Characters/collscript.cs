using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collscript : MonoBehaviour {

    //Este es el objetivo que va a recibir da;o
    public GameObject Objective;
    [HideInInspector]
    public MeeleAttack Attack;
    public int indexAttack;

    void Awake() {
        JsonLoader.LoadData();
        Attack = JsonLoader.attacks.MeeleAttacks[indexAttack];
    }

    void Update() {
        Attack = JsonLoader.attacks.MeeleAttacks[indexAttack];
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Si el objeto colisionado es el objetivo entonces habra un daño
        if (col.gameObject.tag == Objective.tag){
                col.SendMessage("Damaged",Attack);
        }
    }
}
