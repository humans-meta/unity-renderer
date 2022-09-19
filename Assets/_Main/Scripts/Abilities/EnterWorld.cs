using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts;
using _Main.Scripts.Abilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterWorld : Ability {
    private const string ActionName = "Enter World";

    public override bool CanStartAbility() {
        var info = GetAbility<Hover>();
        return base.CanStartAbility()
               && info.IsActive
               && info.DetectedObject.GetComponent<World>() != null
               && playerInput.actions[ActionName].WasPressedThisFrame();
    }

    protected override void AbilityStarted() {
        base.AbilityStarted();

        var info = GetAbility<Hover>();
        var inner = info.DetectedObject.GetComponent<World>();
        var current = inner.Parent;
        var outer = inner.Parent.Parent;

        var scaleFactor = 1 / inner.ParentRef.transform.lossyScale.x;
        var player = m_CharacterLocomotion.transform;
        var spawn = inner.spawn;
        var tf = outer.transform;
        var targetPosition = player.position - scaleFactor * (spawn.position - tf.position);
        // var targetRotation = spawn.rotation;
        // var alpha = Quaternion.FromToRotation(spawn.forward, player.forward);
        // var targetRotation = tf.rotation * alpha;
        // var v = targetPosition - player.position;
        // targetPosition += alpha * v - v;
        var targetScale = tf.localScale * scaleFactor;

        // m_CharacterLocomotion.SetInput(false);
        m_CharacterLocomotion.SetPhysics(false);
        player.SetParent(null);

        EventHandler.ExecuteEvent("OnWorldWillChange", inner);
        inner.GetComponent<WorldTransition>()
            .StartTransition(
                tf, targetPosition, tf.rotation, targetScale,
                () => {
                    current.transform.SetParent(null, true);
                    outer.Disable();
                    player.SetParent(inner.transform, true);
                    WorldHandler.InstantiateWorldRefs(inner);

                    // m_CharacterLocomotion.SetInput(true);
                    m_CharacterLocomotion.SetPhysics(true);

                    StopAbility();

                    EventHandler.ExecuteEvent("OnWorldDidChange", inner);
                });
    }
}