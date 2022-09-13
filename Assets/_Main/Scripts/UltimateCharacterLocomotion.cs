using _Main.Scripts.Abilities;
using UnityEngine;

namespace _Main.Scripts {
    public class UltimateCharacterLocomotion: MonoBehaviour {
        public bool Grounded { get; set; }
        public Vector3 Up { get; set; }

        public bool SingleCast(Vector3 maxDistance, Vector3 zero, int solidObjectLayers, ref RaycastHit hit) {
            throw new System.NotImplementedException();
        }

        public T GetAbility<T>() {
            throw new System.NotImplementedException();
        }
    }
}