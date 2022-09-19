using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Abilities {
    public class Ability {
        protected UltimateCharacterLocomotion m_CharacterLocomotion;
        protected PlayerInput playerInput;

        public void Initialize(UltimateCharacterLocomotion loco) {
            m_CharacterLocomotion = loco;
            playerInput = loco.GetComponent<PlayerInput>();
        }

        public bool IsActive { get; set; }

        public virtual bool CanStartAbility() {
            return true;
        }

        public virtual bool CanStopAbility() {
            return false;
        }

        protected virtual void AbilityStarted() {
            Debug.Log("Ability STARTED: " + this.GetType().Name);
        }

        protected virtual void AbilityStopped() {
            Debug.Log("Ability STOPPED: " + this.GetType().Name);
        }

        public void StartAbility() {
            IsActive = true;
            AbilityStarted();
        }

        public void StopAbility() {
            IsActive = false;
            AbilityStopped();
        }

        public T GetAbility<T>() where T : Ability {
            return m_CharacterLocomotion.GetAbility<T>();
        }
    }
}