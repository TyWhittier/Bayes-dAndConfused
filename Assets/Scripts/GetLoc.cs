//Title: GetLoc.cs
//Author: Tyler T. Whittier
//Date: 11/18/2019
//Comments:This script is the main script that will acquire the data that we are interested in (final cursor position of the CoM object and the seen cursor). 



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GetLoc : MonoBehaviour
{
    public int TrialCount; //Count the amount of trials completed so far
    public Transform CursorObject; // The object that the part. sees
    public Transform CoMObject; // The object the part. uses to control cursor
	public Transform Target; // The target the part. is to put their CoM into
	public Transform StartBox; // The start box where the part. begins

    public List<float> CursorLocX = new List<float>(); // The growing array of final cursor locations.
    public List<float> CursorLocZ = new List<float>();  // The growing array of final cursor locations.

    public List<float> CoMLocX = new List<float>(); // The growing array of final CoM locations.
    public List<float> CoMLocZ = new List<float>(); // The growing array of final CoM locations.
    
	public List<float> TargetLocX = new List<float>(); // The growing array of Target locations.
    public List<float> TargetLocZ = new List<float>(); // The growing array of Target locations.
    
	public List<float> StartLocX = new List<float>(); // The growing array of StartBox locations.
    public List<float> StartLocZ = new List<float>(); // The growing array of StartBox locations.
   
   // Start is called before the first frame update
    void Start()
    {
        TrialCount = 0;//Start at trial 0
       

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CursorLocX.Add(CursorObject.position.x);
            CursorLocZ.Add(CursorObject.position.z);
            CoMLocX.Add(CoMObject.position.x);
            CoMLocZ.Add(CoMObject.position.z);
			TargetLocX.Add(Target.position.x);
            TargetLocZ.Add(Target.position.z);
			StartLocX.Add(StartBox.position.x);
            StartLocZ.Add(StartBox.position.z);


            TrialCount = TrialCount +1;

        }
    }       
}