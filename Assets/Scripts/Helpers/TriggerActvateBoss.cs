using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActvateBoss : MonoBehaviour
{

    public Animator Puerta1, Puerta2;

    public List<BoxCollider2D> ActivationList;

    public bool State;

    private void Update()
    {
        Puerta1.SetBool("Abierta", true);
        Puerta2.SetBool("Abierta", true);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Si el objeto colisionado es el objetivo entonces habra un daño
        if (col.gameObject.tag == "Player")
        {
            foreach (BoxCollider2D GO in ActivationList)
            {
                GO.enabled = State;
            }
            Puerta1.SetBool("Abierta", false);
            Puerta2.SetBool("Abierta", false);
            gameObject.SetActive(false);
        }
    }
}
