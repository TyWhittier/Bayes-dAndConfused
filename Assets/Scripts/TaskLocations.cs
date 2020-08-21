//Title: TargetLocation.cs
//Author: Tyler T. Whittier
//Date: 12/02/2019
//Comments: This script takes the hip height of the participant when they are statically standing at the beginning of the trial and then computes the distance of the target as a percentage of hip height.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskLocations : MonoBehaviour
{
	public float Angle;
	public float PercentHipHeight;
	private float P; 
	public GameObject Hip;
	public GameObject CoM;
	public GameObject TargetLoc;
	public GameObject STARTBOX;
	private float TX;
	private float TZ;
	private float SX;
	private float SZ;
	// Start is called before the first frame update
	void Start()
    {
		P = PercentHipHeight/100;
        TX = ((transform.position.y*Mathf.Cos(Angle* Mathf.Deg2Rad))*P) + STARTBOX.transform.position.x;
		TZ = ((transform.position.y*Mathf.Sin(Angle* Mathf.Deg2Rad))*P) + STARTBOX.transform.position.z;
		SX = CoM.transform.position.x-1f;
		SZ = CoM.transform.position.z;
		Debug.Log("Com Z is " + SZ);

	}

	void Update()
    {
		STARTBOX.transform.position = new Vector3(SX, CoM.transform.position.y, SZ);
		TargetLoc.transform.position = new Vector3(TX, CoM.transform.position.y, TZ);

	}
}
