using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{

    public Animator Puerta1, Puerta2;

    public AiluroPhobia Boss;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Boss.Dead)
        {
            Puerta1.SetBool("Abierta", true);
            Puerta2.SetBool("Abierta", true);
        }
    }
}
