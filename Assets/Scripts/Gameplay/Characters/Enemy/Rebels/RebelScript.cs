using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RebelScript : Human {
    //variable que registra si esta en combate la IA
    bool Incombat;
    //Es el rango de vision de la IA
    public float LookRadius = 10f;

    //Objetivo de la IA
    public GameObject Objective;

    public override void LoadData()
    {
        //BASE
        base.LoadData();
        //Captar el Objetivo(Donovan)
        //Objective = Camera.main.transform.parent.gameObject;
    }

    public override void Movement()
    {
        //BASE
        base.Movement();
        //El giro de la IA
        if (Direction.x != 0)
            transform.localScale = new Vector3(Direction.x, 1, 1);
        //Animacion de caminar de la IA
        anim.SetFloat("Movx", Mathf.Abs(Direction.x));
    }

    public override void UpdateThis()
    {
        //BASE
        base.UpdateThis();
        //Si esta a una distancia minima x, entonces Entrara en combate
        if (Vector2.Distance(transform.position, Objective.transform.position) < LookRadius)
        {
            Incombat = true;
            GameplayActions.PrimaryAction = true;
            Direction = new Vector2(Mathf.Sign(Objective.transform.position.x - transform.position.x), 0);
        }// Si no, Regresara a Idle
        else {
            Incombat = false;
            GameplayActions.PrimaryAction = false;
            Direction = new Vector2(0, 0);
        }
        GameplayActions.PrimaryActionDown = Trigger(GameplayActions.PrimaryAction);
        GameplayActions.ReloadAction = ShootingObject.AmmoDonovan.AmmoInMag <= 0;
        ArmsManager(Objective.transform.position + Vector3.up * 6);
        print(ShootingObject.ShootAttack.Name + "/" + ShootingObject.AmmoDonovan.AmmoInMag + "-" + ShootingObject.AmmoDonovan.TotalAmmo);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,LookRadius);
    }
}
