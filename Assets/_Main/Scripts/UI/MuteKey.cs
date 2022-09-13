using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class MuteKey : MonoBehaviour {
    // private Button _button;
    // private ShortcutButton _shortcutButton;
    // private AgoraManager _agoraManager;
    //
    // private void Awake() {
    //     _shortcutButton = GetComponent<ShortcutButton>();
    //     _button = GetComponent<Button>();
    //     _agoraManager = FindObjectOfType<AgoraManager>();
    //     
    //     EventHandler.RegisterEvent("OnSwitchMute", SwitchMute);
    // }
    //
    // public void SwitchMute() {
    //     _agoraManager.SwitchLocalAudioMute();
    //     _shortcutButton.buttonText = _agoraManager.localAudioMuted ? "UNMUTE" : "MUTE";
    //     _shortcutButton.enabled = false;
    //     _shortcutButton.enabled = true;
    //     LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    //     // _shortcutButton.gameObject.SetActive(false);
    //     // _shortcutButton.gameObject.SetActive(true);
    // }
}