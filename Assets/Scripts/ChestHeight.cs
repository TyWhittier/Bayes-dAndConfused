//FALL 2019
//Author: Ty Whittier
//This script os part of the Bayes VR study and should be attached to any object that 
//should mirror the superior-inferior path of the CoM object attached to the
//participant (target and start box)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestHeight : MonoBehaviour
{
    private float Y; // the up and down component of the CoM object attached to the participant
    public GameObject CoM_object; // The object that should be the mirrored (the CoM object attached to participant)
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        //Y is floating and changes depending on the CoM object
        Y = CoM_object.transform.position.y;
        // only the up and down component of the object mirrors the CoM
        transform.position = new Vector3(transform.position.x, Y, transform.position.z);
    }
}
