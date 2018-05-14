using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plyr_controller : MonoBehaviour {

    public float maxspeed = 5f;
    public float speed = 2f;
    public bool Grounded;
    public float JumpPower = 6.5f;

    private Rigidbody2D rb2d;
    private Animator anim;
    private bool jump;

	// Use this for initialization
	void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown(KeyCode.UpArrow) && Grounded) {
            jump = true;
        }
	}

    void FixedUpdate() {
        float h = Input.GetAxis("Horizontal");
        float newspeed = Mathf.Clamp(rb2d.velocity.x,-maxspeed, maxspeed);
        rb2d.AddForce(Vector2.right * speed * h);
        rb2d.velocity = new Vector2(newspeed,rb2d.velocity.y);

        if (h > 0.1f) {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (h < 0.1f){
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }

        if (jump) {
            rb2d.AddForce(Vector2.up * JumpPower, ForceMode2D.Impulse);
            jump = false;
        }

    }
}
