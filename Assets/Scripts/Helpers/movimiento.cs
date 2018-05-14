using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimiento : MonoBehaviour
{
    Vector2 mov;
    Rigidbody2D rb2d;
    public float speed = 10f;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        mov = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxis("Vertical"));
        rb2d.MovePosition(rb2d.position + mov * speed * Time.deltaTime);
    }
}
