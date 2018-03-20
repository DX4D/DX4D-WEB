using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadOnGameState : MonoBehaviour {
    public bool loadOnce;
    public GameState loadOnState;
    public GameObject[] targets;
    bool cameraShowing;

    // Update is called once per frame
    void Update ()
    {
        if (loadOnce && cameraShowing) this.enabled = false;

        if (targets != null && targets.Length > 0)
        {
            bool activate = (GameStateManager.CurrentGameState == loadOnState);
            if (targets.Length == 1) { targets[0].SetActive(activate); } //FOR PERFORMANCE BOOST
            else
            {
                for (int i = 0; i < targets.Length; i++)
                {
                    targets[i].SetActive(activate);
                }
            }
            cameraShowing = activate;
        }
	}
}