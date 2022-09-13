using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Network.Character {
    /// <summary>
    /// Enables the UltimateCharacterLocomotionHandler to be aware of the character on the network.
    /// </summary>
    public class NetworkCharacterLocomotionHandler: MonoBehaviour {
        [Tooltip("Should the camera be attached to the local player?")] [SerializeField]
        protected bool m_AttachCamera = true;

        private INetworkInfo m_NetworkInfo;
        [System.NonSerialized] private PlayerInput m_PlayerInput;

        /// <summary>
        /// Initializes the default values.
        /// </summary>
        protected void Awake() {
            m_NetworkInfo = GetComponent<INetworkInfo>();
            m_PlayerInput = GetComponent<PlayerInput>();

            EventHandler.RegisterEvent(gameObject, "OnCharacterSpawned", OnCharacterSpawned);
            
            // enabled = false;
            // m_CharacterLocomotion.enabled = false;
        }

        private void OnCharacterSpawned() {
            if (m_NetworkInfo.IsLocalPlayer()) {
                if (m_AttachCamera) {
                    // var camera = Shared.Camera.CameraUtility.FindCamera(gameObject);
                    // if (camera != null) {
                    //     camera.GetComponent<UltimateCharacterController.Camera.CameraController>().Character =
                    //         gameObject;
                    // }
                }
            }
            else {
                // Non-local players will not be controlled with player input.
                enabled = false;
                // m_CharacterLocomotion.enabled = false;
                Destroy(m_PlayerInput);
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