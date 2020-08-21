using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{

    // 
    public Transform cameraTransform;
    public float N_Level;
    private Vector3 Loc;
    private Vector3 Noise;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Loc = cameraTransform.position + new Vector3(-1f, 0,0);
        Noise = new Vector3(Random.Range(-1f,1f)*N_Level, Random.Range(-1f, 1f) * N_Level, Random.Range(-1f, 1f) * N_Level);
        transform.position = Loc + Noise;
        

    }
}
