using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmsScript : MonoBehaviour {
	// Update is called once per frame
	void LateUpdate () {
        if (PauseMenu.GameIsPaused)
            return;
        transform.localRotation = Quaternion.Euler(new Vector3(0,0,-transform.parent.transform.localScale.x*AimScript.angle+90));
	}
}
