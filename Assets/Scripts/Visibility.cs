using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour
{
    // private float TargetX; // X component of vector between target and start box in Meters
    //private float TargetZ; // Z component of vector between target and start box in Meters
    //private int theta; // Target Angle in Degrees
    public GameObject Target_Loc;
    public GameObject Start_Loc;
    public MeshRenderer rend;
    private float XlengtH;
    private float ZlengtH;
    private float XmaX;
    private float XmiN;
    private float ZmaX;
    private float ZmiN;
   
    // Start is called before the first frame update
    void Start()
    {
        XlengtH = (Target_Loc.transform.position.x - Start_Loc.transform.position.x);
        ZlengtH = (Target_Loc.transform.position.z - Start_Loc.transform.position.z);
        XmiN = (Start_Loc.transform.position.x) + (XlengtH * .35f);
        XmaX = (Start_Loc.transform.position.x) + (XlengtH * .65f);
        ZmiN = (Start_Loc.transform.position.z) + (ZlengtH * .35f);
        ZmaX = (Start_Loc.transform.position.z) + (ZlengtH * .65f);


        //TargetX = Target_Loc.transform.position.x - Start_Loc.transform.position.x;
        //TargetZ = Target_Loc.transform.position.z - Start_Loc.transform.position.z;
        Debug.Log("XlengtH is " + XlengtH);
        Debug.Log("ZlengtH is " + ZlengtH);

        Debug.Log("Xmin is " + XmiN);
        Debug.Log("Xmax is " + XmaX);
        Debug.Log("Zmin is " + ZmiN);
        Debug.Log("Zmax is " + ZmaX);
        rend = GetComponent<MeshRenderer>();
        rend.enabled = true;
    }

    public GameObject Cursor; // Allows us to define the cursor object to read and track
    public GameObject CoM;
    // Update is called once per frame
    void Update()
    {
        //Vector3 StartVector = new Vector3(Start_Loc.transform.position.x, 0, Start_Loc.transform.position.z);
        //Vector3 CursorPosition = new Vector3(Cursor.transform.position.x,0, Cursor.transform.position.z);
        float CoMX = (CoM.transform.position.x-1);
        float CoMZ = (CoM.transform.position.z);
       // Debug.Log("CoM X is " + CoMX + " CoM Z is " + CoMZ);
        rend = Cursor.GetComponent<MeshRenderer>();

        if ((Mathf.Abs(CoMX) <= Mathf.Abs(XmiN) || Mathf.Abs(CoMX) >= Mathf.Abs(XmaX)) && (Mathf.Abs(CoMZ) <= Mathf.Abs(ZmiN) || (Mathf.Abs(CoMZ) >= Mathf.Abs(ZmaX))))
        {
            rend.enabled = false;
        }
        else
        {
            rend.enabled = true;

        }
        
    }
}