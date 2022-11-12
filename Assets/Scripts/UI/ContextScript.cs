using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextScript : MonoBehaviour {

    public bool InSentence;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Deactivate() {
        InSentence = false;
        gameObject.SetActive(false);
    }
    
}
