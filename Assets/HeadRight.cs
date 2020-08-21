using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class HeadRight : MonoBehaviour
{
    public GameObject HEad; //Object streaming from Tracker recording the headset location
    public Camera Main; // Main camera under the HMD prefab

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        
    }
}
