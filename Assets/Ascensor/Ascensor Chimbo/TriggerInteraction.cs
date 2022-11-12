using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerInteraction : MonoBehaviour
{
    public bool entrar;
    public GameObject InteractionButton;
    private Image IntButton;

    void Start()
    {
        IntButton=InteractionButton.GetComponent<Image>();
    }

    // Use this for initialization
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            entrar = true;
            IntButton.enabled=true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            entrar = false;
            IntButton.enabled=false;
        }
    }
}
