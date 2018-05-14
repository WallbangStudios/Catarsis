using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

	//what are we following
	public Transform target;

	//zeros out the veolocity
	Vector3 velocity = Vector3.zero;

    public Vector3 Offset;

	// time to follow target
	public float smoothTime = .50f;


	//enable and set the maximun Y value
	public bool YMaxEnabled = false;
	public float YMaxValue = 0;

	//enable and set the minimum Y value
	public bool YMinEnabled = false;
	public float YMinValue = 0;

	//enable and set the maximum X value
	public bool XMaxEnabled = false;
	public float XMaxValue = 0;

	//enable and set the minimum X value
	public bool XMinEnabled = false;
	public float XMinValue = 0;


	void FixedUpdate()
	{
		//target position
		Vector3 targetPos = target.position;

		//vertical
		if (YMinEnabled && YMaxEnabled) {
			targetPos.y = Mathf.Clamp (target.position.y, YMinValue, YMaxValue);
		} else if (YMinEnabled) {
			targetPos.y = Mathf.Clamp (target.position.y, YMinValue, target.position.y); 
			
		} else if (YMaxEnabled) {
			targetPos.y = Mathf.Clamp (target.position.y, target.position.y , YMaxValue); 
		}



		//horizontal
		if (XMinEnabled && XMaxEnabled) {
			targetPos.x = Mathf.Clamp (target.position.x, XMinValue, XMaxValue);
		} else if (XMinEnabled) {
			targetPos.x = Mathf.Clamp (target.position.x, XMinValue, target.position.x); 

		} else if (XMaxEnabled) {
			targetPos.x = Mathf.Clamp (target.position.x, target.position.x , XMaxValue); 
		}





		//align the camera and the targets z position
		targetPos.z = transform.position.z;

		//using smooth damp we will gradually change the camera transform position to the target position based on the camera transform velocity and out smooth time
		transform.position = Vector3.SmoothDamp (transform.position, targetPos + Offset, ref velocity, smoothTime);



	}





}
