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
            RaycastHit hit = default;
            if (m_CharacterLocomotion.SingleCast(-m_CharacterLocomotion.Up * MaxDistance, Vector3.zero, 0, ref hit)) {
                return false;
            }

            Debug.Log("hit = " + hit);

            // if (Physics.Raycast(m_Transform.position, -m_CharacterLocomotion.Up, out var hit,
            //                     MaxDistance, m_CharacterLayerManager.SolidObjectLayers,
            //                     QueryTriggerInteraction.Ignore)) {
            //     return false;
            // }

            return true;
        }

        protected override void AbilityStarted() {
            base.AbilityStarted();

            // var respawner = m_CharacterLocomotion.GetComponent<CharacterRespawner>();
            // var spawn = WorldManager.Instance.CurrentWorld.spawn;
            // SchedulerBase.Schedule(Delay, () => {
            //     if (CanStartAbility()) {
            //         respawner.Respawn(spawn.position, spawn.rotation, true);
            //     }
            //
            //     StopAbility();
            // });
        }
    }
}