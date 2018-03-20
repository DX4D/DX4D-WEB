using UnityEngine;

[RequireComponent(typeof(LinkUI))]
public class UIPanelManager : BaseManager {
    [HideInInspector] LinkUI uiLink;

    [SerializeField] AnchorHorizontal horizontalAlignment;
    [SerializeField] AnchorVertical verticalAlignment;
    [SerializeField] Vector2 panelOffset;
    public int showOrder;

    //LOAD
    public override bool LoadManager() {
        //Debug.Log("(loaded)" + name + (ReadyToLoad ? " readytoload" : "") + (!Loaded ? " notloaded" : "")); //FOR DEBUG
        LoadLinks();
        return (uiLink != null);
    }
    void LoadLinks()
    {
        uiLink = GetComponent<LinkUI>();
    }

    //UPDATE
    public override bool UpdateManager(){
        DX4D.UpdatePanelParent(uiLink.active, uiLink.linked);
        DX4D.AlignPanel(uiLink.active, horizontalAlignment, verticalAlignment, panelOffset);
        DX4D.UpdatePanelOrder(uiLink.active, showOrder);
        return true;
    }
}