using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCameraOnGameState : MonoBehaviour {
    public bool loadOnce;
    public GameState loadOnState;
    public Camera targetCamera;
    bool cameraShowing;

    private void Start() { if (targetCamera == null) { targetCamera = GetComponent<Camera>(); } } //Get attached Camera if we have not assigned one

    // Update is called once per frame
    void Update()
    {
        if (loadOnce && cameraShowing) this.enabled = false;
        if (targetCamera == null) return;
        targetCamera.enabled = cameraShowing = (GameStateManager.CurrentGameState == loadOnState);
    }
}