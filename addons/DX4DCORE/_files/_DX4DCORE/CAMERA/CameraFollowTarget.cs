using UnityEngine;

[RequireComponent(typeof(LinkCamera), typeof(LinkEntity))]
public class CameraFollowTarget : MonoBehaviour {
    [HideInInspector] LinkCamera cameraLink;
    [HideInInspector] LinkEntity entityLink;

    public Vector3 followOffset;

    public float lookVertical;
    public float lookHorizontal;

    private void LateUpdate()
    {
        if (cameraLink == null) cameraLink = GetComponent<LinkCamera>();
        if (entityLink == null) entityLink = GetComponent<LinkEntity>();

        if (cameraLink == null || entityLink == null) { return; }

        if (cameraLink.active == null) { return; }
        if (entityLink.linked == null || UMMO.IsDead(entityLink.linked)) { cameraLink.active.transform.position = Vector3.zero; return; }

        DX4D.Follow(cameraLink.active.transform, entityLink.linked.transform, followOffset);
        LookAt(cameraLink.active, entityLink.linked);
    }

    void LookAt(Camera follower, Entity target) { follower.transform.LookAt((target.transform.position) + target.transform.up * lookVertical + target.transform.right * lookHorizontal); }
}