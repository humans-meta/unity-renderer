using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Network.Character {
    /// <summary>
    /// Enables the UltimateCharacterLocomotionHandler to be aware of the character on the network.
    /// </summary>
    public class NetworkCharacterLocomotionHandler : MonoBehaviour {
        private INetworkInfo m_NetworkInfo;
        [System.NonSerialized] private PlayerInput m_PlayerInput;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private ThirdPersonController m_ThirdPersonController;

        /// <summary>
        /// Initializes the default values.
        /// </summary>
        protected void Awake() {
            m_CharacterLocomotion = GetComponent<UltimateCharacterLocomotion>();
            m_ThirdPersonController = GetComponent<ThirdPersonController>();
            m_NetworkInfo = GetComponent<INetworkInfo>();
            m_PlayerInput = GetComponent<PlayerInput>();

            EventHandler.RegisterEvent(gameObject, "OnCharacterSpawned", OnCharacterSpawned);

            // enabled = false;
            // m_CharacterLocomotion.enabled = false;
        }

        private void OnCharacterSpawned() {
            if (m_NetworkInfo.IsLocalPlayer()) {
                var _playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
                _playerCamera.Follow = m_CharacterLocomotion.cameraRoot;
            }
            else {
                // Non-local players will not be controlled with player input.
                enabled = false;
                m_PlayerInput.enabled = false;
                m_CharacterLocomotion.enabled = false;
                m_ThirdPersonController.enabled = false;
            }
        }

        /// <summary>
        /// Determines if the handler should be enabled.
        /// </summary>
        private void Start() {
        }

        /// <summary>
        /// The character has respawned.
        /// </summary>
        protected void OnRespawn() {
            if (!m_NetworkInfo.IsLocalPlayer()) {
                return;
            }
        }
    }
}