using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowDonovan : MonoBehaviour {

    public Transform Target;
    public Vector2 Offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posincamera = Camera.main.WorldToScreenPoint(Target.position + (Vector3)Offset);
        transform.position = posincamera;
    }
}
