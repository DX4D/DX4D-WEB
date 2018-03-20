using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDelay : MonoBehaviour {
    [Tooltip("DelayRate in seconds")]
    public float delayInSeconds = 0.12f;
    [Tooltip("TargetCamera will autoload if nothing is assigned")]
    public Camera targetCamera;

    void OnValidate() { StartCamera(); }
    void Start()
    {
        if (targetCamera == null) { targetCamera = GetComponent<Camera>(); } //Get attached Camera if we have not assigned one
        StartCamera();
    }

    void StartCamera() { if (targetCamera == null) return; CancelInvoke(); InvokeRepeating("CameraOn", delayInSeconds, delayInSeconds); }

    void CameraOn() { targetCamera.enabled = true; }
    void OnPostRender() { targetCamera.enabled = false; }
}