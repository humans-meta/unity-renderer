using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using _Main.Scripts.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;

public interface IHoverDetectable {
    public bool CanDetect();
}

public class Hover : DetectObjectAbilityBase {
    private float defaultMaxDistance = 10f;

    public float Distance => m_RaycastResult.distance;

    public GameObject OriginalDetectedObject;

    public override bool CanStartAbility() {
        var canStart = base.CanStartAbility();
        if (!canStart) return false;
        OriginalDetectedObject = DetectedObject;

        var config = DetectedObject.GetComponent<HoverConfig>();
        var maxDistance = defaultMaxDistance;
        if (config != null) {
            maxDistance = config.maxDistance;
        }

        canStart = Distance <= maxDistance;
        if (!canStart) return false;

        var hoverDetectable = DetectedObject.GetComponent<IHoverDetectable>();
        if (hoverDetectable != null) {
            canStart = hoverDetectable.CanDetect();
        }

        if (!canStart) return false;

        if (config != null && config.proxyTo != null) {
            DetectedObject = config.proxyTo;
        }

        return canStart;
    }

    public override bool CanStopAbility() {
        var currentDetectedObject = DetectedObject;
        base.CanStartAbility();
        var canStop = OriginalDetectedObject != DetectedObject;
        DetectedObject = currentDetectedObject;
        return canStop;
    }

    protected override void AbilityStarted() {
        base.AbilityStarted();

        EventHandler.ExecuteEvent(DetectedObject, "OnHover", true);
        EventHandler.ExecuteEvent("OnHover", DetectedObject, true);
        if (OriginalDetectedObject != DetectedObject) {
            EventHandler.ExecuteEvent(OriginalDetectedObject, "OnHover", true);
            EventHandler.ExecuteEvent("OnHover", OriginalDetectedObject, true);
        }
    }

    protected override void AbilityStopped() {
        EventHandler.ExecuteEvent(DetectedObject, "OnHover", false);
        EventHandler.ExecuteEvent("OnHover", DetectedObject, false);
        if (OriginalDetectedObject != DetectedObject) {
            EventHandler.ExecuteEvent(OriginalDetectedObject, "OnHover", false);
            EventHandler.ExecuteEvent("OnHover", OriginalDetectedObject, false);
        }

        base.AbilityStopped();
    }
}