using System;
using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using FishNet.Component.Transforming;
using FishNet.Connection;
using FishNet.Object;
using FishNet.Object.Synchronizing;
using FishNet.Transporting;
using UnityEngine;
using EventHandler = _Main.Scripts.EventHandler;

public class CharacterSpawnController : NetworkBehaviour {
    [field: SyncVar(Channel = Channel.Reliable, OnChange = nameof(OnChange_WorldName),
                    ReadPermissions = ReadPermission.ExcludeOwner)]
    private string CurrentWorldName { get; [ServerRpc] set; }

    public World CurrentWorld { get; set; }

    private UltimateCharacterLocomotion _characterLocomotion;

    public override void OnStartClient() {
        base.OnStartClient();

        EventHandler.ExecuteEvent(gameObject, "OnCharacterSpawned");
        if (base.IsOwner) {
            EventHandler.ExecuteEvent("OnLocalCharacterSpawned", gameObject);

            var world = WorldManager.Instance.initialWorldRef.to;
            transform.SetParent(world.transform, true);
            EventHandler.ExecuteEvent("OnWorldDidChange", world);
            // _characterLocomotion.SetPhysics(true);
        }

        gameObject.name = $"Player #{base.OwnerId}";
    }

    private void Awake() {
        _characterLocomotion = GetComponent<UltimateCharacterLocomotion>();
        EventHandler.RegisterEvent<World>("OnWorldDidChange", OnWorldDidChange);
    }

    private void Start() {
        var world = WorldManager.Instance.initialWorldRef.to;
        var spawn = world.spawn;
        transform.SetPositionAndRotation(spawn.position, spawn.rotation);
        // _characterLocomotion.SetPhysics(false);
    }

    private void OnDestroy() {
        EventHandler.UnregisterEvent<World>("OnWorldDidChange", OnWorldDidChange);
    }

    void OnChange_WorldName(string prev, string next, bool isServer) {
        Debug.LogFormat("[OnChange] prev={0}, next={1}, isServer={2}, isOwner={3}", prev, next, isServer, base.IsOwner);
        if (isServer || base.IsOwner) return;

        if (WorldManager.Instance.WorldByName.TryGetValue(next, out var world)) {
            // var current = WorldManager.Instance.currentWorld;
            transform.SetParent(world.transform, false);
            transform.localPosition = world.spawn.localPosition;
        }
    }


    private void OnWorldDidChange(World world) {
        if (!base.IsOwner) return;
        CurrentWorld = world;
        CurrentWorldName = world.name;
    }
}