using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public HUDmanager HUD;
    public GameObject Barra;


    public GameObject Key, Card, Lintern, Door, Door2, Button1, Button2, Button3;

    private TriggerInteraction TriggerKey, TriggerCard, TriggerLintern, TriggerDoor, TriggerDoor2, TriggerButton1, TriggerButton2, TriggerButton3;

    
    private Animator anim, anim2;

    public DonovanController Donovan;

    // Start is called before the first frame update
    void Start()
    {
        TriggerKey=Key.GetComponent<TriggerInteraction>();
        TriggerCard=Card.GetComponent<TriggerInteraction>();
        TriggerLintern=Lintern.GetComponent<TriggerInteraction>();

        TriggerDoor=Door.GetComponentInChildren<TriggerInteraction>();
        TriggerDoor2=Door2.GetComponentInChildren<TriggerInteraction>();

        TriggerButton1= Button1.GetComponent<TriggerInteraction>();
        TriggerButton2= Button2.GetComponent<TriggerInteraction>();
        TriggerButton3= Button3.GetComponent<TriggerInteraction>();

        anim = Door.GetComponent<Animator>();
        anim2 = Door2.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(TriggerKey.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HUD.SetContext("You found a Key");
                Barra.SetActive(true);
                Donovan.CurrentHealth += 2;
                GameManagerDemo.HasKeys=true;

                Key.SetActive(false);
            }
        }
        else if(TriggerCard.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HUD.SetContext("You Found an ID Card");
                Barra.SetActive(true);
                Donovan.CurrentHealth += 2;
                GameManagerDemo.HasCard=true;

                Card.SetActive(false);
            }
        }
        else if(TriggerLintern.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                HUD.SetContext("You found a Pistol");
                Barra.SetActive(true);
                Donovan.CurrentHealth++;
                GameManagerDemo.HasPistol=true;

                Lintern.SetActive(false);
            }
        }

        if(TriggerDoor.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(GameManagerDemo.HasKeys==true)
                {
                    anim.SetBool("Abierta", true);
                    TriggerDoor.gameObject.SetActive(false);
                }
                else
                {
                    HUD.SetContext("You need a key");
                    Barra.SetActive(true);
                }

            }
        }

        if(TriggerDoor2.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(GameManagerDemo.HasKeys==true)
                {
                    anim2.SetBool("Abierta", true);
                    TriggerDoor2.gameObject.SetActive(false);
                }
                else
                {
                    HUD.SetContext("You need a key");
                    Barra.SetActive(true);
                }

            }
        }

        if(TriggerButton1.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(GameManagerDemo.HasCard==true)
                {
                    //anim2.SetBool("Abierta", true);
                    //TriggerDoor2.gameObject.SetActive(false);
                }
                else
                {
                    HUD.SetContext("You need an ID Card");
                    Barra.SetActive(true);
                }

            }
        }
        else if(TriggerButton2.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(GameManagerDemo.HasCard==true)
                {
                    //anim2.SetBool("Abierta", true);
                    //TriggerDoor2.gameObject.SetActive(false);
                }
                else
                {
                    HUD.SetContext("You need an ID Card");
                    Barra.SetActive(true);
                }

            }
        }
        else if(TriggerButton3.entrar)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if(GameManagerDemo.HasCard==true)
                {
                    //anim2.SetBool("Abierta", true);
                    //TriggerDoor2.gameObject.SetActive(false);
                }
                else
                {
                    HUD.SetContext("You need an ID Card");
                    Barra.SetActive(true);
                }

            }
        }
    }
}
