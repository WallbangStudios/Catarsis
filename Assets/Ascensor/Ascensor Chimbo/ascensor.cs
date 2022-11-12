using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ascensor : MonoBehaviour
{

    public GameObject Ascensor;
    public GameObject Donovan;

    [Header("Trigger")]
    public GameObject trigger;
    public GameObject triggerInteraction;
    //los suelos A, B y C son de prueba para este ascensor, va a ir un prefab del suelo del ascensor y los tres seran uno

    [Header("Colliders")]
    public BoxCollider2D ColliderFisico;
    public BoxCollider2D ColliderTiro;


    [Header("Ambient Object")]
    public GameObject[] buttonObject; 
    public GameObject[] lightObject;
    public GameObject[] spriteFloor;
    public GameObject[] door;

    private TriggerInteraction[] button = new TriggerInteraction[4]; 
    private SpriteRenderer[] spritePuerta = new SpriteRenderer[4];

    //colocamos aqui a que target se va a mover que pusimos en el unity
    public Transform[] objetivos;
    int i = 0;

    [Header("Variables")]
    //la velocidad en que se mueve la plataforma
    public float velocidad;
    public float velocidad2;

    private bool subir, bajar, activar, mover, activar2, primerUso, primerUso2;

    //cosas implementadas nuevas
    private bool timer, timerSalir;
    public float tiempoEntrarAscensor, tiempoSalirAscensor;
    private float tiempoBase=0;

    public bool movimientoAscensor;
    //

    public bool puedesInteractuar=true;
    private bool Caminar=true;

    //el animator de donovan
    private Animator anim_Donovan;
    //el trigger para actuar con el ascensor
    private trigger trigger_ascensor;
    //el script de movimiento de donovan
    private DonovanController control_Donovan;

    SpriteRenderer SPRDonovan;

    //este es el script que verifica cuando donovan desaparece dentro del ascensor
    private scriptAsc scriptDesaparecerDonovan1, scriptDesaparecerDonovan2, scriptDesaparecerDonovan3;

    private Animator animBoton1, animBoton2, animBoton3, animPiso1, animPiso2, animPiso3;



    void Start()
    {
        //empieza el ascensor el el objetivo 1
        transform.position = objetivos[i].position;


        anim_Donovan = Donovan.GetComponent<Animator>();
        control_Donovan = Donovan.GetComponent<DonovanController>();
        SPRDonovan = Donovan.GetComponent<SpriteRenderer>();
        trigger_ascensor = Ascensor.GetComponentInChildren<trigger>();

        button[0]=buttonObject[0].GetComponent<TriggerInteraction>(); 
        button[1]=buttonObject[1].GetComponent<TriggerInteraction>(); 
        button[2]=buttonObject[2].GetComponent<TriggerInteraction>(); 

        animBoton1 = lightObject[0].GetComponent<Animator>();
        animBoton2 = lightObject[1].GetComponent<Animator>();
        animBoton3 = lightObject[2].GetComponent<Animator>();

        animPiso1= spriteFloor[0].GetComponent<Animator>();
        animPiso2= spriteFloor[1].GetComponent<Animator>();
        animPiso3= spriteFloor[2].GetComponent<Animator>();

        scriptDesaparecerDonovan1= spriteFloor[0].GetComponent<scriptAsc>();
        scriptDesaparecerDonovan2= spriteFloor[1].GetComponent<scriptAsc>();
        scriptDesaparecerDonovan3= spriteFloor[2].GetComponent<scriptAsc>();

        spritePuerta[0]= door[0].GetComponent<SpriteRenderer>();
        spritePuerta[1]= door[1].GetComponent<SpriteRenderer>();
        spritePuerta[2]= door[2].GetComponent<SpriteRenderer>();



        tiempoBase=tiempoEntrarAscensor;
        tiempoBase=tiempoSalirAscensor;

    }



    private void FixedUpdate()
    {

        if (objetivos != null)
        {

            //se cambia la velocidad a velocidad_nueva porque la funcion "towards" necesita una velocidad delta time
            float velocidad_nueva = velocidad * Time.deltaTime;

            if (subir)
            {

                transform.position = Vector3.MoveTowards(transform.position, objetivos[i].position, velocidad_nueva);
                movimientoAscensor=true;
                animBoton1.SetBool("Moviendo", true);
                animBoton2.SetBool("Moviendo", true);
                animBoton3.SetBool("Moviendo", true);

                if(transform.position==objetivos[i].position)
                {
                    subir=false;
                    movimientoAscensor=false;
                    animBoton1.SetBool("Moviendo", false);
                    animBoton2.SetBool("Moviendo", false);
                    animBoton3.SetBool("Moviendo", false);
                }

            }
            if (bajar)
            {

                transform.position = Vector3.MoveTowards(transform.position, objetivos[i].position, velocidad_nueva);
                movimientoAscensor=true;
                animBoton1.SetBool("Moviendo", true);
                animBoton2.SetBool("Moviendo", true);
                animBoton3.SetBool("Moviendo", true);

                if(transform.position==objetivos[i].position)
                {
                    bajar=false;
                    movimientoAscensor=false;
                    animBoton1.SetBool("Moviendo", false);
                    animBoton2.SetBool("Moviendo", false);
                    animBoton3.SetBool("Moviendo", false);

                }

            }

            //para mover a donovan hasta el centro del ascensor antes que comienze a moverse
            if (mover)
            {

                //para que se mueva donovan hasta el centro del trigger
                float velocidad_nueva_mover = velocidad2 * Time.deltaTime;
                Donovan.transform.position = Vector3.MoveTowards(new Vector3(Donovan.transform.position.x, Donovan.transform.position.y, 0), new Vector3(trigger.transform.position.x, Donovan.transform.position.y, 0), velocidad_nueva_mover);

                //para que siempre este caminando hacia delante al entrar
                if (Donovan.transform.position.x < trigger.transform.position.x)
                {
                    activar2 = false;
                    Donovan.transform.localScale = new Vector3(1, 1, 1);

                }
                else if (Donovan.transform.position.x > trigger.transform.position.x)
                {
                    activar2 = false;
                    Donovan.transform.localScale = new Vector3(-1, 1, 1);

                }
                else if (Donovan.transform.position.x == trigger.transform.position.x && timer==false)
                {
                    //tiempoEntrarAscensor=tiempoBase;
                    activar2 = true;
                }

            }

            if(timer)
            {
                puedesInteractuar=false;

                ColliderFisico.enabled = false;
                ColliderTiro.enabled = false;
                control_Donovan.enabled = false;
                control_Donovan.ShootingObject.ArmsAnim.SetBool("CanShowup", false);
                Camera.main.GetComponent<FollowTarget>().target = control_Donovan.transform;

                //para que haga la animacion de caminar donovan
                anim_Donovan.SetFloat("Movx", 1f);

                tiempoEntrarAscensor-=Time.deltaTime;
                
                if(tiempoEntrarAscensor<=0.0f)
                {
                    //para que deje de hacer la animacion donovan cuando llegue al centro
                    anim_Donovan.SetFloat("Movx", 0f);
                    timer=false;
                    Caminar=false;

                            if(i==0) 
                            { 
                                animPiso1.SetBool("Abierto", false);
                                animPiso2.SetBool("Abierto", false);
                                animPiso3.SetBool("Abierto", false);
                            } 
                            else if(i==1) 
                            { 
                                animPiso1.SetBool("Abierto", false);
                                animPiso2.SetBool("Abierto", false);
                                animPiso3.SetBool("Abierto", false);
                            } 
                            else if(i==2) 
                            { 
                                animPiso1.SetBool("Abierto", false);
                                animPiso2.SetBool("Abierto", false);
                                animPiso3.SetBool("Abierto", false);
                            }

                    puedesInteractuar=true;

                }
            }

            if(timerSalir)
            {
                puedesInteractuar=false;

                if(i==0 && transform.position == objetivos[0].position) 
                { 
                    animPiso1.SetBool("Abierto", true);
                    animPiso2.SetBool("Abierto", false);
                    animPiso3.SetBool("Abierto", false);
                } 
                else if(i==1 && transform.position == objetivos[1].position) 
                { 
                    animPiso1.SetBool("Abierto", false);
                    animPiso2.SetBool("Abierto", true);
                    animPiso3.SetBool("Abierto", false);
                } 
                else if(i==2 && transform.position == objetivos[2].position) 
                { 
                    animPiso1.SetBool("Abierto", false);
                    animPiso2.SetBool("Abierto", false);
                    animPiso3.SetBool("Abierto", true);
                }

                tiempoSalirAscensor-=Time.deltaTime;
                
                if(tiempoSalirAscensor<=0.0f)
                {
                    //para que deje de hacer la animacion donovan cuando llegue al centro
                    anim_Donovan.SetFloat("Movx", 0f);
                    timerSalir=false;
                    Caminar=true;
                    SPRDonovan.sortingOrder = 1;

                    control_Donovan.enabled = true;

                    ColliderFisico.enabled = true;
                    ColliderTiro.enabled = true;

                    puedesInteractuar=true;
                }
            }
        }
    }



    private void Update()
    {
        if(GameManagerDemo.HasCard==true)
        {

            //si el trigger del ascensor es true, entonces puede interactuar
        if (trigger_ascensor.entrar_ascensor && puedesInteractuar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                    //para que haga la accion solo si esta en cualquiera de los tres puntos
                    if (transform.position == objetivos[0].position || transform.position == objetivos[1].position || transform.position == objetivos[2].position)
                    {
                        if(scriptDesaparecerDonovan1.donovanDesactivado || scriptDesaparecerDonovan2.donovanDesactivado || scriptDesaparecerDonovan3.donovanDesactivado)
                        {
                            mover=false;
                            activar=false;
                        }
                        if (!scriptDesaparecerDonovan1.donovanDesactivado && !scriptDesaparecerDonovan2.donovanDesactivado && !scriptDesaparecerDonovan3.donovanDesactivado)
                        {
                            mover=true;
                            activar=true;
                        }
                        //para desactivar el movimiento de donovan
                        //mover = !mover;

                        //mover automaticamente al centro
                        if(mover)
                        {
                            tiempoEntrarAscensor=tiempoBase;
                            triggerInteraction.SetActive(false);
                            if(Caminar)
                            {
                                timer=true;
                            }
                            SPRDonovan.sortingOrder = 1;
                        }
                        else
                        {
                            tiempoSalirAscensor=tiempoBase;
                            triggerInteraction.SetActive(true);
                            timerSalir=true;
                            anim_Donovan.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        }

                        //activar = !activar;

                        //desactivar el script de movimiento de donovan
                        //control_Donovan.enabled = !control_Donovan.enabled;

                        //para que donovan se quede quieto al entrar al ascensor
                        anim_Donovan.SetFloat("Movx", 0f);

                        Donovan.transform.parent = null;


                    }

                }

        


            if (Input.GetButtonDown("Up") && activar && activar2)
            {
                if (transform.position == objetivos[0].position || transform.position == objetivos[1].position || transform.position == objetivos[2].position)
                {
                    if(scriptDesaparecerDonovan1.donovanDesactivado || scriptDesaparecerDonovan2.donovanDesactivado || scriptDesaparecerDonovan3.donovanDesactivado)
                    {
                        if (i < 2)
                        {
                            bajar = false;
                            subir = true;

                            i = i + 1;

                            Donovan.transform.parent = Ascensor.transform;
                        }
                    }
                }
            }

            if (Input.GetButtonDown("Down") && activar && activar2)
            {
                if (transform.position == objetivos[0].position || transform.position == objetivos[1].position || transform.position == objetivos[2].position)
                {
                    if(scriptDesaparecerDonovan1.donovanDesactivado || scriptDesaparecerDonovan2.donovanDesactivado || scriptDesaparecerDonovan3.donovanDesactivado)
                    {
                        if (i > 0)
                        {

                            subir = false;
                            bajar = true;

                            i = i - 1;

                            Donovan.transform.parent = Ascensor.transform;
                        }
                    }
                }
            }
        }

        if(button[0].entrar) 
        { 
            if (Input.GetKeyDown(KeyCode.E)) 
            { 
                i=0; 
                subir=false; 
                bajar=true; 
                animBoton1.SetBool("Activado", true);
                animBoton2.SetBool("Activado", true);
                animBoton3.SetBool("Activado", true);

                triggerInteraction.SetActive(true);
                trigger.SetActive(true);

                if(primerUso==false)
                {
                    animPiso1.SetBool("Abierto", true);
                }

                primerUso=true;
                primerUso2=true;
            } 
        } 
        else if(button[1].entrar) 
        { 
            if (Input.GetKeyDown(KeyCode.E)) 
            { 
                i=1; 
                subir=false; 
                bajar=true; 
                animBoton1.SetBool("Activado", true);
                animBoton2.SetBool("Activado", true);
                animBoton3.SetBool("Activado", true);

                triggerInteraction.SetActive(true);

            } 
        } 
        else if(button[2].entrar) 
        { 
            if (Input.GetKeyDown(KeyCode.E)) 
            { 
                i=2; 
                subir=false; 
                bajar=true;
                animBoton1.SetBool("Activado", true); 
                animBoton2.SetBool("Activado", true);
                animBoton3.SetBool("Activado", true);

                triggerInteraction.SetActive(true);
            } 
        } 
 
        if(i==0) 
        { 
            if(primerUso)
            {
                buttonObject[0].SetActive(false);
                trigger.gameObject.GetComponent<BoxCollider2D>().enabled=true;
            }
            else
            {
                buttonObject[0].SetActive(true);
            }
 
            buttonObject[1].SetActive(true); 
            buttonObject[2].SetActive(true);

            animBoton1.SetBool("Actual", false);
            animBoton2.SetBool("Actual", false);
            animBoton3.SetBool("Actual", true);
            
        } 
        else if(i==1) 
        { 
            trigger.gameObject.GetComponent<BoxCollider2D>().enabled=true;

            buttonObject[0].SetActive(true); 
            buttonObject[1].SetActive(false); 
            buttonObject[2].SetActive(true); 

            animBoton1.SetBool("Actual", false);
            animBoton2.SetBool("Actual", true);
            animBoton3.SetBool("Actual", false);

            if(transform.position == objetivos[1].position && primerUso2==false)
            {
                animPiso2.SetBool("Abierto", true);

                primerUso=true;
                primerUso2=true;
            }
        } 
        else if(i==2) 
        { 
            buttonObject[0].SetActive(true); 
            buttonObject[1].SetActive(true); 
            buttonObject[2].SetActive(false); 

            animBoton1.SetBool("Actual", true);
            animBoton2.SetBool("Actual", false);
            animBoton3.SetBool("Actual", false);
        } 
        }
    }
}
