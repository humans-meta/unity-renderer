using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using FishNet;
using FishNet.Managing;
using FishNet.Object;
using UnityEngine;
using EventHandler = _Main.Scripts.EventHandler;

public class WorldManager : MonoBehaviour {
    public static WorldManager Instance;
    public WorldRef initialWorldRef;
    public World CurrentWorld { get; private set; }
    public readonly Dictionary<string, World> WorldByName = new();

    private CinemachineVirtualCamera virtualCamera;

    public Transform LocalPlayer {
        get {
            if (virtualCamera == null) {
                virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
            }
            return virtualCamera.Follow != null ? virtualCamera.Follow.parent : null;
        }
    }


    private void Awake() {
        Instance = this;

        foreach (var world in FindObjectsOfType<World>()) {
            WorldByName.Add(world.name, world);
        }

        CurrentWorld = initialWorldRef.to;
        EventHandler.RegisterEvent<World>("OnWorldWillChange", OnWorldWillChange);
        EventHandler.RegisterEvent<World>("OnWorldDidChange", OnWorldDidChange);
    }

    private void OnWorldWillChange(World obj) {
    }


    private void OnWorldDidChange(World world) {
        CurrentWorld.OnExit();
        CurrentWorld = world;
        CurrentWorld.OnEnter();
    }

    private void Start() {
        WorldHandler.InstantiateFromRef(initialWorldRef);
        WorldHandler.DisableUnlinked(initialWorldRef.to);
    }
}