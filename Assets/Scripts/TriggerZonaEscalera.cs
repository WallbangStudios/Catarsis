using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZonaEscalera : MonoBehaviour
{
    public bool entrar_zona { get; private set; }

    public bool salir_zona { get; private set; }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            entrar_zona=true;
            salir_zona=false;
        }
        
    }
	    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Player")
        {
            entrar_zona=false;
            salir_zona=true;
        }
    }
}
