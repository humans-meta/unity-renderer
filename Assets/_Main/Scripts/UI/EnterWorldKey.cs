using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using UnityEngine;
using EventHandler = _Main.Scripts.EventHandler;
using UnityEngine.UI;

public class EnterWorldKey : MonoBehaviour {
    private Button _button;

    private void Awake() {
        _button = GetComponent<Button>();
        EventHandler.RegisterEvent<GameObject, bool>("OnHover", OnHover);
    }

    private void OnHover(GameObject go, bool active) {
        var loco = LocalPlayerManager.Instance.Character.GetComponent<UltimateCharacterLocomotion>();
        var enterWorld = loco.GetAbility<EnterWorld>();
        _button.interactable = enterWorld.CanStartAbility();
    }
}