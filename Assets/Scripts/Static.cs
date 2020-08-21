using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Static : MonoBehaviour
{
    // Start is called before the first frame update
   
    public Transform Object;
    public float N_Level;
    private Vector3 Loc;
    private Vector3 noise;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Loc = Object.position;
        noise = new Vector3(Random.Range(-1f, 1f) * N_Level, Random.Range(-1f, 1f) * N_Level, Random.Range(-1f, 1f) * N_Level);
        transform.position = Loc + noise;

    }

   
}
