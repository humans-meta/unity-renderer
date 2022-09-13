using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EventHandler = _Main.Scripts.EventHandler;

public class EnableOnHover : MonoBehaviour {
    public GameObject target;

    private void Awake() {
        EventHandler.RegisterEvent<bool>(gameObject, "OnHover", OnHover_Enable);
        target.gameObject.SetActive(false);
    }

    private void OnDestroy() {
        EventHandler.UnregisterEvent<bool>(gameObject, "OnHover", OnHover_Enable);
    }

    private void OnHover_Enable(bool active) {
        target.gameObject.SetActive(active);
    }
}