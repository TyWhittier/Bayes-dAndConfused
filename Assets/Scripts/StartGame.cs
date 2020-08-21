using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartGame : MonoBehaviour
{

    public float timeLeft = 3.0f;
    public Text startText; // used for showing countdown from 3, 2, 1 


    void Update()
    {
		timeLeft -= Time.deltaTime;
        startText.text = (timeLeft).ToString("0");
        if (timeLeft < 0.01)
        {
		   GameObject varGameObject = GameObject.Find("Canvas"); 
		    
			varGameObject.GetComponent<Canvas>().enabled = false;
        }
    }
}