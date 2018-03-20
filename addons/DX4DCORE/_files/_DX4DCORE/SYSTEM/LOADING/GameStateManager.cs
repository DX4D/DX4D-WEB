using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    #region INSTANCE
    public static GameStateManager instance { get; private set; }
    void Awake() { if (instance == null) { instance = this; } else { Debug.Log("There can be only one: " + this); } }
    #endregion

    public GameObject loginPanel;
    public GameObject characterSelectPanel;
    public GameObject characterCreationPanel;

    void OnGUI() { GUI.Label(new Rect((Screen.width / 2), 92, 255, 20), GameStateManager.CurrentGameState.ToString()); } //FOR DEBUG

    void Start() { instance.enabled = true; }
    void Update() { if (playerLoaded) this.enabled = false; playerLoaded = Utils.ClientLocalPlayer() != null; }

    bool playerLoaded;
    public static GameState CurrentGameState
    {
        get
        {
            if (instance.playerLoaded) { return GameState.Playing; }
            else if (instance.characterCreationPanel.activeSelf) { return GameState.CharacterCreation; }
            else if (instance.characterSelectPanel.activeSelf) { return GameState.CharacterSelect; }
            else if (instance.loginPanel.activeSelf) { return GameState.Login; }
            else { return GameState.Loading; }
        }
    }
}