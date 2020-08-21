using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerDirections : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject text = new GameObject();
        TextMesh t = text.AddComponent<TextMesh>();
        t.text = "Trigger = Select, A = New Trial, B = End Trial";
        t.fontSize = 4;
        t.transform.localEulerAngles += new Vector3(0, 90, 0);
        t.transform.localPosition += new Vector3(0f, 3f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
