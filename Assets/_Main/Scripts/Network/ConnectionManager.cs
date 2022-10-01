﻿using System;
using System.Collections;
using System.Collections.Generic;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;

public class ConnectionManager : MonoBehaviour {
    private NetworkManager _networkManager;

    private void Awake() {
        _networkManager = FindObjectOfType<NetworkManager>();
    }

    private void Start() {
#if !UNITY_SERVER
#if UNITY_EDITOR && false
        try {
            _networkManager.ServerManager.StartConnection();
        }
        catch {
            // ignored
        }
#else
        _networkManager.ClientManager.StartConnection();
#endif
#endif
    }
}