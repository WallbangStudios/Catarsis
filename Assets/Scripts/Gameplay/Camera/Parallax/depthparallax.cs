using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class depthparallax : MonoBehaviour {

    public Camera main;
    public float size;

	// Use this for initialization
	void Start () {
      
	}
	
	// Update is called once per frame
    void Update () {
        float angle = Mathf.Deg2Rad * main.fieldOfView/2;
        float ratio = size / (2 * Mathf.Tan(angle));
        float newscale = (transform.position.z - main.transform.position.z) / ratio;
        transform.localScale = new Vector3(newscale,newscale,1);
	}
}
