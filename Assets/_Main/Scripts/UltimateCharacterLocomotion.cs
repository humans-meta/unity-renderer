using System;
using System.Collections.Generic;
using _Main.Scripts.Abilities;
using UnityEngine;

namespace _Main.Scripts {
    public class UltimateCharacterLocomotion : MonoBehaviour {
        public bool Grounded { get; set; }
        public Vector3 Up { get; set; }

        private List<Ability> _abilities;

        private void Awake() {
            _abilities = new List<Ability> {
                new Hover(),
                new EnterWorld(),
                new ExitWorld()
            };
            foreach (var ability in _abilities) {
                ability.Initialize(this);
            }
        }

        public bool SingleCast(Vector3 maxDistance, Vector3 zero, int solidObjectLayers, ref RaycastHit hit) {
            throw new System.NotImplementedException();
        }

        public T GetAbility<T>() where T : Ability {
            var type = typeof(T);
            foreach (var ability in _abilities) {
                if (type.IsInstanceOfType(ability)) {
                    return ability as T;
                }
            }

            return null;
        }

        private void Update() {
            foreach (var ability in _abilities) {
                if (!ability.IsActive && ability.CanStartAbility()) {
                    ability.StartAbility();
                }
                else if (ability.IsActive && ability.CanStopAbility()) {
                    ability.StopAbility();
                }
            }
        }
    }
}