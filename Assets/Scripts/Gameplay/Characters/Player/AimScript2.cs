using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimScript2 : MonoBehaviour {

    Vector2 point = new Vector2(1, 1);

    public GameObject Donovan;

    public ParallaxElement[] elements;

    [Range(0, 360)]
    public float angletest = 90;

    [Range(0, 180)]
    public float FieldOfFire = 25f;
    public float radiusofFire = 2;

    float TempField;

    [HideInInspector]
    public float Shootangle;
    Vector3 viewShootAngle;

    public Vector3 DirFromAngle(float AngleInDegrees, bool isGlobal)
    {
        if (!isGlobal)
        {
            AngleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(AngleInDegrees * Mathf.Deg2Rad), Mathf.Cos(AngleInDegrees * Mathf.Deg2Rad), 0);
    }

    // Use this for initialization
    void Start()
    {
        TempField = FieldOfFire;
        Shootangle = angletest;
        viewShootAngle = DirFromAngle(Shootangle, false);
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 vect = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        angletest = Vector2.Angle(transform.up, vect);

        if (Input.GetKey(KeyCode.LeftShift))
        {
            FieldOfFire = 1;
        }
        else
        {
            FieldOfFire = TempField;
        }
        float angle = angletest * Donovan.transform.localScale.x;
        float Aalpha = angle + FieldOfFire / 2;
        float Balpha = angle - FieldOfFire / 2;
        Vector3 ViewAngleA = DirFromAngle(Aalpha, false);
        Vector3 ViewAngleB = DirFromAngle(Balpha, false);
        Vector3 ViewAngleTest = DirFromAngle(angle, false);
        if (Input.GetKeyDown(KeyCode.T))
        {
            Shootangle = Random.Range(Aalpha, Balpha);
            viewShootAngle = DirFromAngle(Shootangle, false);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, viewShootAngle, radiusofFire);
            if (hit != null)
            {
                Destroy(hit.transform.gameObject);
            }
        }

        Debug.DrawLine(transform.position, transform.position + ViewAngleA * radiusofFire, Color.white);
        Debug.DrawLine(transform.position, transform.position + ViewAngleB * radiusofFire, Color.white);
        Debug.DrawLine(transform.position, transform.position + viewShootAngle * radiusofFire, Color.red);
        //RaycastHit2D hit;
        //Physics2D.Raycast(transform.position,);

    }

    [System.Serializable]
    public struct ParallaxElement {
        public Transform transforms;
        public float scale;
    }

}
