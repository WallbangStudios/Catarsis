using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotaScript : MonoBehaviour
{

    public DialogueManager Manager;
    public Dialogue DialogMessage;

    TriggerInteraction Interaccion;

    // Start is called before the first frame update
    void Start()
    {
        Interaccion = GetComponent<TriggerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!DialogueManager.InConversation && Interaccion.entrar && Input.GetKeyDown(KeyCode.E)) {
            Manager.StartConversation(DialogMessage, this);
        }
    }
}
