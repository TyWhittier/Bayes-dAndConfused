using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MasterScript : MonoBehaviour
{
  

    public GameObject GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GetComponent<ProtocolManager>().enabled = false;
       
    }

    // Update is called once per frame
   void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        if (SteamVR_Actions._default.Start.GetStateUp(SteamVR_Input_Sources.Any))
        {
            GameManager.GetComponent<ProtocolManager>().enabled  = true;
            //Trialcount = Trialcount + 1;

        }

        //if (Input.GetKeyDown(KeyCode.A))
        if (SteamVR_Actions._default.Reset.GetStateUp(SteamVR_Input_Sources.Any))

        {
            GameManager.GetComponent<ProtocolManager>().enabled = false;

        }
       


    }
}
