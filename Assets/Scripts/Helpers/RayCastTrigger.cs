using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastTrigger : MonoBehaviour {

    public float Width, Height;

    public bool ON;

    public LayerMask Layer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ON = Physics2D.BoxCast(transform.position, new Vector2(Width, Height), 0, Vector2.zero);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector2(Width, Height));
    }
}
