using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFreeze : MonoBehaviour {

    [Tooltip("DelayRate in seconds")]
    public float freezeInSeconds = 1.0f;
    [Tooltip("TargetCamera will autoload if nothing is assigned")]
    public Camera targetCamera;

    void OnValidate() { } //StartCamera(); }
    void Start()
    {
        if (targetCamera == null) { targetCamera = GetComponent<Camera>(); } //Get attached Camera if we have not assigned one
        StopCamera();
    }

    void StopCamera() { if (targetCamera == null) return; CancelInvoke(); Invoke("CameraOff", freezeInSeconds); }

    void CameraOff() { targetCamera.enabled = false; }
    void OnPostRender() { } //targetCamera.enabled = false; }
}