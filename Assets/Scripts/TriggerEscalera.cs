using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEscalera : MonoBehaviour {

    public bool subir_escalera { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            subir_escalera=true;
        }
        
    }
	    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            subir_escalera=false;
        }
    }
}
