using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour {

    public State CurrentState;
    public State remainstate;

    private bool IAactive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (!IAactive)
            return;
        CurrentState.UpdateState(this);

	}

    public void TransitionToState(State nextstate) {
        if (nextstate != remainstate) {
            CurrentState = nextstate;
        }
    }
}
