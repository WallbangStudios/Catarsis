using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour {

    [Tooltip("Aqui van los backgrounds y sus diferentes escalas")]
    public ParallaxElement[] Elements;

    public float Smoothing;

    public bool freezey, freezex;


    private Transform Came;
    private Vector3 PreviousCamPos;

	// Use this for initialization
	void Start () {

        Came = Camera.main.transform;

        PreviousCamPos = Came.position;

        Smoothing = -Smoothing;
		
	}
	
	// Update is called once per frame
	void Update () {

        for (int i = 0; i < Elements.Length; i++) {
            Vector3 Parallax = (PreviousCamPos - Came.position) * (Elements[i].ParallaxScale / Smoothing);

            float x, y, z;

            y = Elements[i].Background.position.y;
            x = Elements[i].Background.position.x;
            z = Elements[i].Background.position.z;

            if (!freezey)
                y += Parallax.y;

            if (!freezex)
                x += Parallax.x;

            Elements[i].Background.position = new Vector3(x, y, z);
        }
        PreviousCamPos = Came.position;

	}

    [System.Serializable]
    public struct ParallaxElement {
        public Transform Background;
        public float ParallaxScale;
    }

}
