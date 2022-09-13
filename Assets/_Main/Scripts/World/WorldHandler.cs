using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldHandler {
    public static void InstantiateRef(WorldRef worldRef) {
        // var current = worldRef.self;
        var inner = worldRef.to;
        // var refTf = worldRef.transform;

        inner.ParentRef = worldRef;
        // inner.transform.SetParent(current.transform);

        // inner.transform.localScale = Vector3.one / inner.scaleFactor;
        // inner.transform.SetPositionAndRotation(refTf.position, refTf.rotation);
        inner.transform.localPosition = Vector3.zero;
        inner.transform.localRotation = Quaternion.identity;
        inner.transform.SetParent(worldRef.transform, false);
        inner.gameObject.SetActive(true);
    }

    public static void InstantiateWorldRefs(World world) {
        foreach (var worldRef in world.Refs) {
            if (worldRef.gameObject.activeSelf) {
                InstantiateRef(worldRef);
            }
        }
    }

    public static void InstantiateFromRef(WorldRef worldRef) {
        var outer = worldRef.self;
        var current = worldRef.to;
        var scaleFactor = 1 / worldRef.transform.lossyScale.x;

        InstantiateRef(worldRef);
        outer.transform.localScale = Vector3.one * scaleFactor;

        InstantiateWorldRefs(current);
    }

    public static void DisableUnlinked(World self) {
        var innerWorlds = new HashSet<World>();
        foreach (var worldRef in self.Refs) {
            innerWorlds.Add(worldRef.to);
        }

        foreach (var other in Object.FindObjectsOfType<World>()) {
            if (other != self && other != self.Parent && !innerWorlds.Contains(other)) {
                other.Disable();
            }
        }
    }
}