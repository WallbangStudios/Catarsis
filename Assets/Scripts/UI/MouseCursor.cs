using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
	}
	
	// Update is called once per frame
	void Update () {
        /*
        Vector2 CursorPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        GetComponent<RectTransform>().position = CursorPos;
         */
        if (Cursor.visible)
            Cursor.visible = false;
        transform.position = Input.mousePosition;
	}
}
