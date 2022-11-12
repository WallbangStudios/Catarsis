using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escalera : MonoBehaviour {

    public DonovanController Donovan;

	[Header("Colliders")]
	public BoxCollider2D escalera;
    public BoxCollider2D sueloPisoArriba;

    [Header("Trigger")]
    public TriggerEscalera escaleraEntrada;
    public TriggerEscalera escaleraSalida;
    public TriggerEscalera subiendoEscalera;
    public TriggerEscalera atrasEscalera;

    [Header("Zones")]
    public GameObject zonaAbajo;
    public GameObject zonaArriba;

    private TriggerZonaEscalera escaleraAbajo;
    private TriggerZonaEscalera escaleraArriba;

    [Header("Direction Stairs")]
    [Range(-1,1)]
    public int direction;

    private bool escaleraDesactivada, escaleraActivada;


	// Use this for initialization
	void Start () {
		escaleraAbajo = zonaAbajo.GetComponent<TriggerZonaEscalera>();
        escaleraArriba = zonaArriba.GetComponent<TriggerZonaEscalera>();
	}
	
	// Update is called once per frame
	void Update () {

        if (escaleraEntrada.subir_escalera)
        {
            if(Input.GetButton("Down") && Input.GetAxisRaw("Horizontal") == direction)
            {
                escaleraActivada=false;
                escaleraDesactivada=true;
            }
        }
        else if (escaleraSalida.subir_escalera)
        {
            if(Input.GetButton("Down") && Input.GetAxisRaw("Horizontal") == -direction)
            {
                escaleraDesactivada=false;
                escaleraActivada=true;
                sueloPisoArriba.enabled=false;
            }
        }

        if(escaleraAbajo.entrar_zona)
        {
            escaleraActivada=false;
            escaleraDesactivada=true;
            escalera.enabled=false;
        }
        if(escaleraAbajo.salir_zona && zonaAbajo.active==true)
        {
            escaleraDesactivada=false;
            escaleraActivada=true;
            escalera.enabled=true;
        }
        if (escaleraArriba.entrar_zona)
        {
            escaleraDesactivada=false;
            escaleraActivada=true;
            sueloPisoArriba.enabled=false;
        }
        if(escaleraArriba.salir_zona && zonaArriba.active==true)
        {
            escaleraActivada=false;
            zonaArriba.SetActive(false);
            sueloPisoArriba.enabled=true;
        }


        if(escaleraDesactivada)
        {
            zonaAbajo.SetActive(true);
            zonaArriba.SetActive(false);
        }
        else if(escaleraActivada)
        {
            zonaAbajo.SetActive(false);
            zonaArriba.SetActive(true);
        }



        if(subiendoEscalera.subir_escalera)
        {
            escaleraDesactivada=false;
            escaleraActivada=true;
        }

        if(atrasEscalera.subir_escalera)
        {
            escaleraActivada=false;
            escaleraDesactivada=true;
        }

	}

}
