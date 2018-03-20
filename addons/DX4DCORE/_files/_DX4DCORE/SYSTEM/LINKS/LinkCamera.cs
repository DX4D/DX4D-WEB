using UnityEngine;

[RequireComponent(typeof(Camera))]
public class LinkCamera : MonoBehaviour
{
    //    [HideInInspector] Camera linkedCamera; [HideInInspector] 
    
    public Camera active;

    void Start() { DoLoad(); }
    void OnValidated() { DoLoad(); }

    void LateUpdate() { DoLoad(); }

    //LOAD
    void DoLoad() { LoadLinks(); }
    void LoadLinks() { LoadCamera(); }
    void LoadCamera() { if (active == null) active = GetComponent<Camera>(); }
}