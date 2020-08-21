//Title: TargetLocation.cs
//Author: Tyler T. Whittier
//Date: 12/02/2019
//Comments: This script takes the hip height of the participant when they are statically standing at the beginning of the trial and then computes the distance of the target as a percentage of hip height.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocation : MonoBehaviour
{
	public float Angle;
	public float PercentHipHeight;
	private float P; 
	public GameObject Hip;
	public GameObject TargetLoc;
	public GameObject STARTBOX;
	private float X;
	private float Z;

    // Start is called before the first frame update
    void Start()
    {
		P = PercentHipHeight/100;
        X = ((transform.position.y*Mathf.Cos(Angle* Mathf.Deg2Rad))*P) + STARTBOX.transform.position.x;
		Z = ((transform.position.y*Mathf.Sin(Angle* Mathf.Deg2Rad))*P) + STARTBOX.transform.position.z;
		TargetLoc.transform.position = new Vector3(X,transform.position.y,Z);
		
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
