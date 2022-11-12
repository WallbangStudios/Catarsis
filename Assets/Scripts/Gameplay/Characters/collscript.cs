using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collscript : MonoBehaviour {

    //Este es el objetivo que va a recibir da;o
    public string ObjectiveTag;
    [HideInInspector]
    public Attacks Attack;
    public bool Edit;
    public MeeleAttack MAttack;
    public int indexAttack;

    public Impact Impulse;

    void Start() {
        if (Edit) {
            Attack.MeeleAttacks[indexAttack].Damage = MAttack.Damage;
            JsonLoader<Attacks>.UpdateData(Attack, "Atacks");
        }
        else {
            Attack = JsonLoader<Attacks>.LoadData("Atacks");
            MAttack = Attack.MeeleAttacks[indexAttack];
        }
    }

    void Update() {
        if (Edit)
            return;
        Attack = JsonLoader<Attacks>.LoadData("Atacks");
        MAttack = Attack.MeeleAttacks[indexAttack];
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Si el objeto colisionado es el objetivo entonces habra un daño
        if (col.gameObject.tag == ObjectiveTag){
            col.transform.parent.gameObject.SendMessage("Damaged", MAttack);
            if(MAttack.Name == "Impulse") {
                float direction = Impulse.ImpulseMagnitude * Mathf.Sign(transform.parent.position.x - col.transform.parent.position.x);
                Impulse.ImpulseMagnitude = direction;
                col.transform.parent.gameObject.SendMessage("Impulse", Impulse);
            }
            if (MAttack.Name == "Atomosfobia")
            {
                float direction = Impulse.ImpulseMagnitude * transform.parent.localScale.x;
                Impulse.ImpulseMagnitude = direction;
                col.transform.parent.gameObject.SendMessage("Impulse", Impulse);
            }
        }
    }
}
