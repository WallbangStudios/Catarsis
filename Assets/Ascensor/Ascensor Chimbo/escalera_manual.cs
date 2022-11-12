using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escalera_manual : MonoBehaviour {

public GameObject Player;
    public GameObject Escalera;

    public GameObject ParedArriba, ParedAbajo;

    public Transform[] objetivos;

    public trigger_manual trigger, trigger2;

    private Animator anim_Donovan;

    private DonovanController control_Donovan;

    private bool EntrarAbajo, EntrarArriba, SalirAbajo, SalirArriba; 
    private bool activar= false;
    private bool subir, bajar= false;

    public float velocidad_movimiento=0f;
    int i = 0;


    private SpriteRenderer ParedArribaOrden, ParedAbajoOrden;

    // Use this for initialization
    void Start()
    {
        transform.position= objetivos[i].position;
        anim_Donovan = Player.GetComponent<Animator>();
        control_Donovan = Player.GetComponent<DonovanController>();

        ParedAbajoOrden= ParedAbajo.GetComponent<SpriteRenderer>();
        ParedArribaOrden= ParedArriba.GetComponent<SpriteRenderer>();
    }


private void FixedUpdate()
{
    if (EntrarAbajo)
    {
        //para que haga la animacion de caminar donovan
        anim_Donovan.SetFloat("Movx", 1f);

        //para que se mueva donovan hasta el centro del trigger
        float velocidad_nueva_mover = velocidad_movimiento * Time.deltaTime;
        Player.transform.position = Vector3.MoveTowards(new Vector3(Player.transform.position.x, Player.transform.position.y, 0), new Vector3(ParedAbajo.transform.position.x, Player.transform.position.y, 0), velocidad_nueva_mover);

        //para colocar a donovan despues del muro y despues se vuelve a la normalidad
        ParedAbajoOrden.sortingOrder=5;
        ParedArribaOrden.sortingOrder=5;

        //para que siempre este caminando hacia delante al entrar
        if (Player.transform.position.x < ParedAbajo.transform.position.x)
        {
            activar = false;
            Player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Player.transform.position.x > ParedAbajo.transform.position.x)
        {
            activar = false;
            Player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Player.transform.position.x == ParedAbajo.transform.position.x)
        {
            activar = true;
        }
    }
    if (EntrarArriba)
    {
       //para que haga la animacion de caminar donovan
       anim_Donovan.SetFloat("Movx", 1f);

        //para que se mueva donovan hasta el centro del trigger
        float velocidad_nueva_mover = velocidad_movimiento * Time.deltaTime;
        Player.transform.position = Vector3.MoveTowards(new Vector3(Player.transform.position.x, Player.transform.position.y, 0), new Vector3(ParedArriba.transform.position.x, Player.transform.position.y, 0), velocidad_nueva_mover);

        //para colocar a donovan despues del muro y despues se vuelve a la normalidad
        ParedAbajoOrden.sortingOrder=5;
        ParedArribaOrden.sortingOrder=5;

        //para que siempre este caminando hacia delante al entrar
        if (Player.transform.position.x < ParedArriba.transform.position.x)
        {
            activar = false;
            Player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Player.transform.position.x > ParedArriba.transform.position.x)
        {
            activar = false;
            Player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Player.transform.position.x == ParedArriba.transform.position.x)
        {
            activar = true;
        }
    }

    if(SalirArriba)
    {
        anim_Donovan.SetFloat("Movx", 1f);

        //para que se mueva donovan hasta el centro del trigger
        float velocidad_nueva_mover = velocidad_movimiento * Time.deltaTime;
        Player.transform.position = Vector3.MoveTowards(new Vector3(Player.transform.position.x, Player.transform.position.y, 0), new Vector3(trigger2.transform.position.x, Player.transform.position.y, 0), velocidad_nueva_mover);


        //para que siempre este caminando hacia delante al entrar
        if (Player.transform.position.x < trigger2.transform.position.x)
        {
            Player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Player.transform.position.x > trigger2.transform.position.x)
        {
            Player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Player.transform.position.x == trigger2.transform.position.x)
        {
            //para que deje de hacer la animacion donovan cuando llegue al centro
            anim_Donovan.SetFloat("Movx", 0f);

            //donovan vuelve a estar por delante de los muros
            ParedAbajoOrden.sortingOrder=0;
            ParedArribaOrden.sortingOrder=0;

            control_Donovan.enabled = true;
            Player.transform.parent = null;

            SalirArriba=false;
        }
    }
    if(SalirAbajo)
    {
        anim_Donovan.SetFloat("Movx", 1f);

        //para que se mueva donovan hasta el centro del trigger
        float velocidad_nueva_mover = velocidad_movimiento * Time.deltaTime;
        Player.transform.position = Vector3.MoveTowards(new Vector3(Player.transform.position.x, Player.transform.position.y, 0), new Vector3(trigger.transform.position.x, Player.transform.position.y, 0), velocidad_nueva_mover);


        //para que siempre este caminando hacia delante al entrar
        if (Player.transform.position.x < trigger.transform.position.x)
        {
            Player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (Player.transform.position.x > trigger.transform.position.x)
        {
            Player.transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (Player.transform.position.x == trigger.transform.position.x)
        {
            //para que deje de hacer la animacion donovan cuando llegue al centro
            anim_Donovan.SetFloat("Movx", 0f);

            //donovan vuelve a estar por delante de los muros
            ParedAbajoOrden.sortingOrder=0;
            ParedArribaOrden.sortingOrder=0;

            control_Donovan.enabled = true;
            Player.transform.parent = null;

            SalirAbajo=false;
        }
    }

}


    // Update is called once per frame
    private void Update()
    {

        if (trigger.entrar_ascensor)
        {
            //control_Donovan.Canjump=false;
                if (Input.GetKeyDown(KeyCode.E))
                {
                        if (i < 1)
                        {                                                   
                            //desactivar el script de movimiento de donovan
                            control_Donovan.enabled = false;
                            //anim_Donovan.SetFloat("Movx", 0f);

                            subir= true;

                        }
                }
        }
        else if(trigger2.entrar_ascensor)
        {
            if (Input.GetKeyDown(KeyCode.E))
                {
                    if (i > 0)
                    {
                        control_Donovan.enabled = false;
                        //anim_Donovan.SetFloat("Movx", 0f);
                        
                        bajar= true;


                    }
                }
        }
                if(subir)
                {
                    EntrarAbajo= true;

                    if(activar==true)
                    {
                        //parent del ascensor el player
                        Player.transform.parent = Escalera.transform;

                        i = i + 1;
                        transform.position = objetivos[i].position;

                        EntrarAbajo= false;
                        activar=false;
                        //control_Donovan.enabled = true;
                        Player.transform.parent = null;
                        SalirArriba=true;

                        subir=false;

                    }

                }
                else if (bajar)
                {
                    EntrarArriba= true;

                    if(activar==true)
                    {
                        //parent del ascensor el player
                        Player.transform.parent = Escalera.transform;

                        i = i - 1;
                        transform.position = objetivos[i].position;

                        EntrarArriba=false;
                        activar=false;
                        //control_Donovan.enabled = true;
                        Player.transform.parent = null;
                        SalirAbajo=true;

                        bajar=false;
                    }
                }


    }
}
