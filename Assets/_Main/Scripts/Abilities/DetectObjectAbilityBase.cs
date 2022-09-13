using UnityEngine;
using UnityEngine.EventSystems;

namespace _Main.Scripts.Abilities {
    public class DetectObjectAbilityBase : Ability {
        protected RaycastResult m_RaycastResult;
        public GameObject DetectedObject { get; set; }
    }
}