using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using UnityEngine;

public class LocalPlayerManager : MonoBehaviour {
    public static LocalPlayerManager Instance;
    public GameObject Character { get; private set; }

    private void Awake() {
        Instance = this;
        EventHandler.RegisterEvent<GameObject>("OnLocalCharacterSpawned", OnLocalCharacterSpawned);
    }

    private void OnLocalCharacterSpawned(GameObject character) {
        Character = character;
    }
}