using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class check_ground : MonoBehaviour {

    public DonovanController player;

	// Use this for initialization
	void Start () {
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Cover" && player.GameplayActions.RunAction) {
            player.Tempdirection = player.Direction.x;
        }
    }

    void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "Ground"){
            player.Grounded = true;
        }
        if (col.gameObject.tag == "Cover" && player.GameplayActions.RunAction) {
            player.OnCover = true;
        }

    }

    void OnCollisionExit2D(Collision2D col){
        if (col.gameObject.tag == "Ground"){
            player.Grounded = false;
        }
        if (col.gameObject.tag == "Cover") {
            player.OnCover = false;
        }

    }
}
