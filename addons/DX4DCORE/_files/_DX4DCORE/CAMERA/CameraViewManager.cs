using UnityEngine;

[RequireComponent(typeof(LinkCamera), typeof(LinkEntity))]
public class CameraViewManager : MonoBehaviour {
    [HideInInspector] LinkCamera cameraLink;
    [HideInInspector] LinkEntity entityLink;

    [SerializeField] CameraViewport view;

    //LAYERS
    [SerializeField] LayerMask layersToShow;
    private LayerMask showLayers;

    //CONFIG
    [SerializeField] bool UMA;
    [SerializeField] bool NPC;
    [SerializeField] bool showEquipment;

    //POSITION
    private Vector3 followOffset;
    [SerializeField] Vector3 cameraOffset;
    [SerializeField] Vector3 cameraOffsetWhileMoving;
    [SerializeField] Vector3 cameraOffsetWhileCasting;

    //VIEW
    [SerializeField] float lookVertical;
    [SerializeField] float lookHorizontal;
    [SerializeField] float cameraZoom;
    [SerializeField] float cameraFieldOfView;

    //LOAD
    void Awake() { layersToShow = LayerMask.GetMask("Player", "Npc", "Monster", "Pet"); LoadLinks(); }
    void LoadLinks() {
        if (cameraLink == null) cameraLink = GetComponent<LinkCamera>();
        if (entityLink == null) entityLink = GetComponent<LinkEntity>();
    }

    //START
    void OnValidate() { Configure(); }
    void Start() { Configure(); }
    void Configure() {
        ConfigureCameraView();
        ConfigureCameraLayers();
    }

    //UPDATE
    void Update() {
        LoadLinks();

        if (cameraLink == null || entityLink == null) { return; }
        if (cameraLink.active == null) { return; }

        followOffset = GetOffsets();
        DX4D.ChangeCameraView(cameraLink.active, cameraZoom, cameraFieldOfView, showLayers);

        if (entityLink.linked == null || UMMO.IsDead(entityLink.linked)) { cameraLink.active.transform.position = Vector3.zero; return; }

        DX4D.Follow(cameraLink.active.transform, entityLink.linked.transform, followOffset);
        ConfigureCameraLookAt(cameraLink.active, entityLink.linked);
    }

    //CONFIG
    Vector3 GetOffsets()
    {
        if (entityLink == null || entityLink.linked == null) return cameraOffset;

        return (cameraOffset + (UMMO.IsMoving(entityLink.linked) ? cameraOffsetWhileMoving : Vector3.zero) + (UMMO.IsCasting(entityLink.linked) ? cameraOffsetWhileCasting : Vector3.zero));
    }
    void ConfigureCameraView() {
        switch (view)
        {
            case CameraViewport.Custom:
                {
                    break;
                }
            case CameraViewport.Face:
                {
                    if (UMA)
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.54f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 0.24f;
                        cameraFieldOfView = 28.0f;

                        //offset
                        cameraOffsetWhileMoving.y = -0.48f;
                        cameraOffsetWhileCasting.y = 0.0f;
                    }
                    else if (NPC)
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.81f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 0.28f;
                        cameraFieldOfView = 33.0f;

                        //offset
                        cameraOffsetWhileMoving.y = -0.4f;
                        cameraOffsetWhileCasting.y = -0.35f;
                    }
                    else
                    {
                        //position
                        cameraOffset.x = 0.3f;
                        cameraOffset.y = 2.0f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 0.18f;
                        cameraFieldOfView = 28.0f;

                        //offset
                        cameraOffsetWhileMoving.y = -0.58f;
                        cameraOffsetWhileCasting.y = -0.12f;
                    }
                    break;
                }
            case CameraViewport.Bust:
                {
                    if (UMA)
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.6f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 0.3f;
                        cameraFieldOfView = 48.0f;

                        //offset
                        cameraOffsetWhileMoving.y = 0.0f;
                        cameraOffsetWhileCasting.y = 0.0f;
                    }
                    else if (NPC)
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.7f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 0.6f;
                        cameraFieldOfView = 48.0f;

                        //offset
                        cameraOffsetWhileMoving.y = -0.1f;
                        cameraOffsetWhileCasting.y = -0.62f;
                    }
                    else
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.7f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 0.6f;
                        cameraFieldOfView = 48.0f;

                        //offset
                        cameraOffsetWhileMoving.y = -0.1f;
                        cameraOffsetWhileCasting.y = -0.2f;
                    }
                    break;
                }
            case CameraViewport.Body:
                {
                    if (UMA)
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.2f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 1.0f;
                        cameraFieldOfView = 100.0f;

                        //offset
                        cameraOffsetWhileMoving.y = 0.0f;
                        cameraOffsetWhileCasting.y = 0.0f;
                    }
                    else
                    {
                        //position
                        cameraOffset.x = 0.0f;
                        cameraOffset.y = 1.1f;
                        cameraOffset.z = 1.0f;

                        //view
                        cameraZoom = 1.1f;
                        cameraFieldOfView = 100.0f;

                        //offset
                        cameraOffsetWhileMoving.y = 0.0f;
                        cameraOffsetWhileCasting.y = 0.0f;
                    }
                    break;
                }
            default:
                { //This never gets called, but it's here just in case
                    //position
                    cameraOffset.x = 0.0f;
                    cameraOffset.y = 1.0f;
                    cameraOffset.z = 1.0f;

                    //view
                    cameraZoom = 1.0f;
                    cameraFieldOfView = 100.0f;

                    //offset
                    cameraOffsetWhileMoving.y = 0.0f;
                    cameraOffsetWhileCasting.y = 0.0f;
                    break;
                }
        }
        //lookHorizontal = cameraOffset.x;
        lookVertical = cameraOffset.y;
    }
    void ConfigureCameraLayers() {
        LayerMask layers = layersToShow;
        if (showEquipment) layers += LayerMask.GetMask("Equipment");
        if (showEquipment && !UMA) layers += LayerMask.GetMask("Default");
        showLayers = layers;
    }
    void ConfigureCameraLookAt(Camera follower, Entity target) {
        follower.transform.LookAt((target.transform.position) + target.transform.up * lookVertical + target.transform.right * lookHorizontal);
    }
}

//(1 << LayerMask.NameToLayer(layerName));
//void ShowLayers(LayerMask layers) { showLayers += layers; }
//void HideLayers(LayerMask layers) { showLayers -= layers; }
//void ShowLayer(LayerMask layer, string layerName) { layer += (1 << LayerMask.NameToLayer(layerName)); }
//void HideLayer(LayerMask layer, string layerName) { layer -= (1 << LayerMask.NameToLayer(layerName)); }