using System;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts.Abilities {
    public class Fallen : Ability {
        public float MaxDistance = 100f;
        public float Delay = 3f;

        public override bool CanStartAbility() {
            // An attribute may prevent the ability from starting.
            if (!base.CanStartAbility()) {
                return false;
            }

            // Fall can't be started if the character is on the ground.
            if (m_CharacterLocomotion.Grounded) {
                return false;
            }

            // Check whether there is ground underneath
            var tf = m_CharacterLocomotion.transform;
            var offset = new Vector3(0, 0.1f, 0);
            var characterController = m_CharacterLocomotion.GetComponent<CharacterController>();
            if (Physics.SphereCast(tf.position + offset, characterController.radius, -tf.up,
                                   out var hit, MaxDistance, m_CharacterLocomotion.SolidObjectLayers)) {
                return false;
            }

            return true;
        }

        protected override void AbilityStarted() {
            base.AbilityStarted();

            var spawn = WorldManager.Instance.CurrentWorld.spawn;
            Scheduler.Schedule(Delay, () => {
                if (CanStartAbility()) {
                    m_CharacterLocomotion.Respawn(spawn.position, spawn.rotation);
                }

                StopAbility();
            });
        }
    }
}