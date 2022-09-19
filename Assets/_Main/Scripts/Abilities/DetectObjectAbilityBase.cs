using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Main.Scripts.Abilities {
    public class DetectObjectAbilityBase : Ability {
        protected RaycastHit m_RaycastResult;
        public float m_CastDistance = 10f;
        public LayerMask m_DetectLayers = LayerMask.GetMask("Interactable");
        public QueryTriggerInteraction m_TriggerInteraction = QueryTriggerInteraction.Collide;
        public GameObject DetectedObject { get; set; }

        public override bool CanStartAbility() {
            if (!base.CanStartAbility())
                return false;

            var camera = Camera.main;
            if (Physics.Raycast(camera.transform.position, camera.transform.forward, out m_RaycastResult,
                                m_CastDistance, m_DetectLayers, m_TriggerInteraction)) {
                if (ValidateObject(m_RaycastResult)) {
                    DetectedObject = m_RaycastResult.collider.gameObject;
                    // Debug.Log("DetectObjectAbilityBase/CanStartAbility: " + DetectedObject.name);
                    return true;
                }
            }

            DetectedObject = null;
            return false;
        }

        private bool ValidateObject(RaycastHit mRaycastResult) {
            return true;
        }
    }
}