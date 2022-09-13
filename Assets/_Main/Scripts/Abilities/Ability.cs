namespace _Main.Scripts.Abilities {
    public class Ability {
        protected UltimateCharacterLocomotion m_CharacterLocomotion;
        public bool IsActive { get; set; }
        
        public virtual bool CanStartAbility() {
            return true;
        }

        public virtual bool CanStopAbility() {
            return true;
        }

        protected virtual void AbilityStarted() {
        }

        protected virtual void AbilityStopped(bool force) {
        }

        protected virtual void StopAbility() {
        }

        public T GetAbility<T>() {
            return m_CharacterLocomotion.GetAbility<T>();
        }
    }
}