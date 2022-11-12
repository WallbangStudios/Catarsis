using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerActivador : MonoBehaviour
{

    public List<Animator> Doors;
    public List<GameObject> ActivationList;

    public bool State;

    private void Update()
    {
        if (Doors.Count != 0)
        {
            foreach (Animator Door in Doors)
            {
                Door.SetBool("Abierta", true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Si el objeto colisionado es el objetivo entonces habra un daño
        if (col.gameObject.tag == "Player")
        {
            if (ActivationList.Count != 0)
            {
                foreach (GameObject GO in ActivationList)
                {
                    GO.SetActive(State);
                }
            }
            if(Doors.Count != 0) {
                foreach (Animator Door in Doors) {
                    Door.SetBool("Abierta", false);
                }
            }
            gameObject.SetActive(false);
        }
    }
}
