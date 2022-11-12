using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deactivate : MonoBehaviour {

	Character Object;

	// Use this for initialization
	void Start () {
		Object = GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.C))
			Object.enabled = !Object.enabled;
	}
}
