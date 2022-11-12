using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarkRoomManager : MonoBehaviour {

    public TanatofobiaEU Tanato1, tanato2;
    public int numbersdeath;

    public Animator puerta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (tanato2 != null)
        {
            if (Tanato1 == null && !tanato2.gameObject.activeSelf)
            {
                tanato2.gameObject.SetActive(true);
            }
        }
        if (numbersdeath >= 2) {
            puerta.SetBool("Abierta", true);
        }
    }
}
