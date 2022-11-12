using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptAsc : MonoBehaviour
{
    public SpriteRenderer D;

    public bool donovanDesactivado { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(D.enabled==false)
        {
            donovanDesactivado=true;
        }
        else
        {
            donovanDesactivado=false;
        }
    }

    public void Disablesprite() {
        D.enabled = false;
    }
}
