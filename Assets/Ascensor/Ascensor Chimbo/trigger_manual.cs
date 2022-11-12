using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger_manual : MonoBehaviour {


    public GameObject Player;

    [HideInInspector]
    public DonovanController control_donovan_salto;

    public bool entrar_ascensor;

    void Start()
    {

        control_donovan_salto = Player.GetComponent<DonovanController>();
    }


    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            control_donovan_salto.Canjump=false;
            entrar_ascensor = true;
  
            print(control_donovan_salto.Canjump+"salta");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            entrar_ascensor = false;
            //control_donovan_salto.Canjump=true;
            print(control_donovan_salto.Canjump+"salta");
        }
    }
}
