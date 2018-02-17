using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDelayRender : MonoBehaviour {
    public float FPS = 5f;
    public Camera renderCam;
    // Use this for initialization
    void Start()
    {
        InvokeRepeating("Render", 0f, 1f / FPS);
    }
    void OnDestroy()
    {
        //CancelInvoke ();
    }
    void Render()
    {
        renderCam.enabled = true;
    }
    void OnPostRender()
    {
        renderCam.enabled = false;
    }
}
