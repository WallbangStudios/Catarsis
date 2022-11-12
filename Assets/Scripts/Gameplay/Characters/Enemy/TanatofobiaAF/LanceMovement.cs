using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanceMovement : MonoBehaviour {

	public float Speed;

	public TanatofobioAfr Dueño;

	public Vector2 Direction;

	// Update is called once per frame
	void Update () {


		transform.Translate(Direction * Speed * Time.deltaTime);




	}
}
