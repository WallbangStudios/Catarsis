using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oscilador : MonoBehaviour {
    public float IntMinIntensity;
    public float IntMaxIntensity;
    public float IntMinrange;
    public float IntMaxRange;
    public Light fogata;
	float time;
    float RandIntensity;
    	// Use this for initialization
	void Start () {
        fogata = GetComponent<Light>();
		RandIntensity = Random.Range(IntMinIntensity,IntMaxIntensity);
	}
	
	// Update is called once per frame
	void Update () {
		//time += Time.deltaTime;
		//if (time>0.5) {
			RandIntensity = Random.Range(IntMinIntensity,IntMaxIntensity);
        //    time = 0;
		//}
        //int RandRange = Random.Range(IntMinrange,IntMaxRange);
        fogata.intensity = RandIntensity;
        //fogata.range = RandRange;
	}
}
