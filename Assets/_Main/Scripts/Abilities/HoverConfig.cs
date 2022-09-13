using System;
using UnityEngine;

public class HoverConfig : MonoBehaviour {
    public float maxDistance = 3f;
    public GameObject proxyTo;

    private void Awake() {
        if (proxyTo != null) {
            var world = proxyTo.GetComponent<World>();
            if (world != null) {
                world.proxies.Add(this);
            }
        }
    }
}