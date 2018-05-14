using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScript : MonoBehaviour {

    [Tooltip("Aqui van los backgrounds y sus diferentes escalas")]
    public ParallaxElement[] Elements;

    public float Smoothing;


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

            Elements[i].Background.position = new Vector3(Elements[i].Background.position.x + Parallax.x, 
                                                            Elements[i].Background.position.y + Parallax.y, 
                                                                Elements[i].Background.position.z);
        }
        PreviousCamPos = Came.position;

	}

    [System.Serializable]
    public struct ParallaxElement {
        public Transform Background;
        public float ParallaxScale;
    }

}
