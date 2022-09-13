using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Main.Scripts;
using _Main.Scripts.Abilities;
using UnityEngine;

public class ExitWorld : Ability {
    public override bool CanStartAbility() {
        var spawnController = m_CharacterLocomotion.GetComponent<CharacterSpawnController>();
        var hasGrandParent = spawnController.CurrentWorld.Parent.ParentRef != null;
        return base.CanStartAbility() && hasGrandParent;
    }

    protected override void AbilityStarted() {
        base.AbilityStarted();

        var spawnController = m_CharacterLocomotion.GetComponent<CharacterSpawnController>();
        var current = spawnController.CurrentWorld;
        var outer = current.Parent;
        var grandOuter = outer.Parent;

        var player = m_CharacterLocomotion.transform;
        var spawn = outer.spawn;
        var scaleFactor = 1 / outer.transform.localScale.x;
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

        EventHandler.ExecuteEvent("OnWorldWillChange", outer);
        current.GetComponent<WorldTransition>()
            .StartTransition(
                tf, targetPosition, tf.rotation, targetScale,
                () => {
                    foreach (var r in current.Refs) {
                        r.to.Disable();
                    }

                    var tf = grandOuter.transform;
                    var spawn = outer.ParentRef.transform;
                    var scaleFactor = 1 / outer.ParentRef.transform.lossyScale.x;
                    tf.position = outer.transform.position - scaleFactor * (spawn.position - tf.position) / tf.localScale.x;
                    tf.localScale = Vector3.one * scaleFactor;
                    grandOuter.gameObject.SetActive(true);

                    // WorldHandler.InstantiateWorldRefs(grandOuter);
                    outer.transform.SetParent(outer.ParentRef.transform, true);

                    // m_CharacterLocomotion.SetInput(true);
                    m_CharacterLocomotion.SetPhysics(true);
                    player.SetParent(outer.transform, true);

                    StopAbility();

                    EventHandler.ExecuteEvent<World>("OnWorldDidChange", outer);
                });
    }
}