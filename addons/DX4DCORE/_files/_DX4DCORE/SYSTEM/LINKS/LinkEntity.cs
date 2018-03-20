using UnityEngine;

public class LinkEntity : MonoBehaviour
{
    public Entity active;
    public Entity linked {
        get {
            if (linkToTarget && active != null)
            {
                return active.target;
                //if (active.target != null) { return active.target; } else { return null; }
            }
            else { return active; }
        }
        set { active = value; }
    }
    public bool linkToTarget;

    bool isLoaded;

    void Start() { DoLoad(); }
    void OnValidated() { DoLoad(); }

    void LateUpdate() { DoLoad(); }

    //LOAD
    void DoLoad() { LoadLinks(); }
    void LoadLinks() { LoadPlayer(); }
    void LoadPlayer()
    {
        if (!isLoaded) active = Utils.ClientLocalPlayer();
        if (!isLoaded) isLoaded = (active != null);

        if (isLoaded && active == null) active = Utils.ClientLocalPlayer();
    }
}