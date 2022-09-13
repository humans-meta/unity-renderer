using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = _Main.Scripts.EventHandler;

public class HighlightOnHover : MonoBehaviour {
    public int defaultLayer = 0;
    public int activeLayer = 7;
    
    private GameObject target;

    private void Awake() {
        if (target == null) {
            target = gameObject;
        }
        EventHandler.RegisterEvent<bool>(gameObject, "OnHover", OnHover_SetLayer);
    }

    private void OnDestroy() {
        EventHandler.UnregisterEvent<bool>(gameObject, "OnHover", OnHover_SetLayer);
    }

    private void OnHover_SetLayer(bool active) {
        var layer = active ? activeLayer : defaultLayer;
        foreach (Transform child in target.GetComponentsInChildren<Transform>()) {
            if (child.gameObject.layer == defaultLayer ||
                child.gameObject.layer == activeLayer) {
                child.gameObject.layer = layer;
            }
        }
    }
}