using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventHandler = _Main.Scripts.EventHandler;

public class ExitWorldKey : MonoBehaviour {
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        EventHandler.RegisterEvent<World>("OnWorldDidChange", OnWorldDidChange);
    }

    private void OnWorldDidChange(World world) {
        _button.interactable = CanExitWorld(world);
    }

    private static bool CanExitWorld(World world) {
        return world.Parent.ParentRef != null;
    }
}