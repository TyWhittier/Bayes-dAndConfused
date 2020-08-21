using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follow : MonoBehaviour
{
    public GameObject StartBox;
    public GameObject Target;
    public GameObject Cursor;
    public GameObject Cursor_FB;
    public GameObject CoM;
    // Start is called before the first frame update
    void OnEnable()
    {
        Vector3 offset = Quaternion.AngleAxis(0, CoM.transform.up) * CoM.transform.forward * .5f;
       // Vector3 Global = transform.TransformPoint(Vector3.right * 2);
        Target.transform.position = CoM.transform.position + offset;
        StartBox.transform.position = CoM.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
