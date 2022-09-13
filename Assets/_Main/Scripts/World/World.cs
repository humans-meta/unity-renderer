using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(WorldTransition))]
public class World : MonoBehaviour, IHoverDetectable {
    public Transform spawn;

    [NonSerialized] public WorldRef[] Refs;
    public WorldRef ParentRef { get; set; }
    public World Parent => ParentRef.self;
    public List<HoverConfig> proxies = new();

    public void OnEnter() {
        EnableCollider(false);

        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null) StartCoroutine(audioSource.FadeIn(1, 1));
    }

    public void OnExit() {
        EnableCollider(true);

        var audioSource = GetComponent<AudioSource>();
        if (audioSource != null) StartCoroutine(audioSource.FadeOut(2.5f));
    }

    private void Awake() {
        Refs = GetComponentsInChildren<WorldRef>()
            .Where(worldRef => worldRef.self == this)
            .ToArray();
    }

    public bool IsRef(World other) {
        return Refs.Any(worldRef => worldRef.to == other);
    }

    public bool IsOuter() {
        return WorldManager.Instance.CurrentWorld.Parent == this;
    }

    public bool IsCurrent() {
        return WorldManager.Instance.CurrentWorld == this;
    }


    public bool IsInner() {
        return gameObject.activeSelf && !IsCurrent() && !IsOuter();
    }

    public void Disable() {
        gameObject.SetActive(false);
        transform.SetParent(null);
        transform.localScale = Vector3.one;
    }

    public bool CanDetect() {
        return IsInner();
    }

    public void EnableCollider(bool value) {
        var coll = GetComponent<Collider>();
        if (coll != null) coll.enabled = value;

        foreach (var proxy in proxies) {
            var proxyColl = proxy.GetComponent<Collider>();
            if (proxyColl != null) proxyColl.enabled = value;
        }
    }
}