using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionMovement : MonoBehaviour {

    public Transform Panel1, Panel2, Panel3;

    float DistanceUnitX;

	// Use this for initialization
	void Start () {
        bool comp = Panel1.localScale.x ==  Panel2.localScale.x && 
                    Panel2.localScale.x ==  Panel3.localScale.x &&
                    Panel3.localScale.x ==  Panel1.localScale.x;
        if (comp)
        {
            DistanceUnitX = Panel1.localScale.x;
        }
        else {
            Debug.LogError("Los paneles no son iguales en tamaño");
        }
	}
    public Transform PanelMedio;
	// Update is called once per frame
	void Update () {
        float P1 = Panel1.position.x, P2 = Panel2.position.x, P3 = Panel3.position.x;
        PanelMedio = CalcularMedio(Panel1, Panel2, Panel3);
        Transform Camer = Camera.main.transform;
        if (Camer.position.x > (PanelMedio.position.x + (DistanceUnitX / 2))) {
            if (PanelMedio == Panel1)
            {
                Relocalizar(Panel3);
            }
            else if (PanelMedio == Panel2)
            {
                Relocalizar(Panel1);
            }
            else {
                Relocalizar(Panel2);
            }
        }
        else if (Camer.position.x < (PanelMedio.position.x - (DistanceUnitX / 2)))
        {
            if (PanelMedio == Panel1)
            {
                Relocalizar(Panel2);
            }
            else if (PanelMedio == Panel2)
            {
                Relocalizar(Panel3);
            }
            else
            {
                Relocalizar(Panel1);
            }
        }
	}

    public void Relocalizar(Transform PM) {
        float P1 = Panel1.position.x, P2 = Panel2.position.x, P3 = Panel3.position.x;
        if (PM == Panel1)
        {
            Panel3.position = new Vector3(P1 + DistanceUnitX, Panel3.position.y, Panel3.position.z);
            Panel2.position = new Vector3(P1 - DistanceUnitX, Panel2.position.y, Panel2.position.z);
        }
        else if (PM == Panel2)
        {
            Panel1.position = new Vector3(P2 + DistanceUnitX, Panel1.position.y, Panel1.position.z);
            Panel3.position = new Vector3(P2 - DistanceUnitX, Panel3.position.y, Panel3.position.z);
        }
        else if (PM == Panel3)
        {
            Panel2.position = new Vector3(P3 + DistanceUnitX, Panel2.position.y, Panel2.position.z);
            Panel1.position = new Vector3(P3 - DistanceUnitX, Panel1.position.y, Panel1.position.z);
        }
    }

    Transform CalcularMedio(Transform Pa1, Transform Pa2, Transform Pa3) {
        float P1 = Pa1.position.x, P2 = Pa2.position.x, P3 = Pa3.position.x;

        if ((P2 < P1 && P1 < P3) || (P2 > P1 && P1 > P3))
        {
            return Pa1;
        }
        else if ((P1 < P2 && P2 < P3) || (P1 > P2 && P2 > P3))
        {
            return Pa2;
        }
        else
        {
            return Pa3;
        }
    }
}
