using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using UnityEngine;

public class AgoraController : MonoBehaviour {
    public void OnSwitchMute() {
        EventHandler.ExecuteEvent("OnSwitchMute");
    }
}