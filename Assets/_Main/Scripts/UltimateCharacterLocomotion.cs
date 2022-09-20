using System;
using System.Collections.Generic;
using _Main.Scripts.Abilities;
using UnityEngine;

namespace _Main.Scripts {
    public class UltimateCharacterLocomotion : MonoBehaviour {
        public int SolidObjectLayers;
        public bool Grounded { get; set; }
        public Vector3 Up { get; set; }

        private List<Ability> _abilities;
        private CharacterController _characterController;

        private void Awake() {
            _characterController = GetComponent<CharacterController>();
            SolidObjectLayers = LayerMask.GetMask("Default");
            _abilities = new List<Ability> {
                new Hover(),
                new EnterWorld(),
                new ExitWorld(),
                new Fallen(),
            };
            foreach (var ability in _abilities) {
                ability.Initialize(this);
            }
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
            Grounded = _characterController.isGrounded;
            foreach (var ability in _abilities) {
                if (!ability.IsActive && ability.CanStartAbility()) {
                    ability.StartAbility();
                }
                else if (ability.IsActive && ability.CanStopAbility()) {
                    ability.StopAbility();
                }
            }
        }

        public void Respawn(Vector3 spawnPosition, Quaternion spawnRotation) {
            transform.position = spawnPosition;
            transform.rotation = spawnRotation;
        }
    }
}