using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Main.Scripts.Network.Character {
    public class NetworkCharacterLocomotionHandler : MonoBehaviour {
        private INetworkInfo networkInfo;
        private PlayerInput playerInput;
        private UltimateCharacterLocomotion characterLocomotion;
        private ThirdPersonController thirdPersonController;

        protected void Awake() {
            characterLocomotion = GetComponent<UltimateCharacterLocomotion>();
            thirdPersonController = GetComponent<ThirdPersonController>();
            networkInfo = GetComponent<INetworkInfo>();
            playerInput = GetComponent<PlayerInput>();

            EventHandler.RegisterEvent(gameObject, "OnCharacterSpawned", OnCharacterSpawned);
        }

        private void OnCharacterSpawned() {
            if (networkInfo.IsLocalPlayer()) {
                var playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
                playerCamera.Follow = characterLocomotion.cameraRoot;
            }
            else {
                enabled = false;
                playerInput.enabled = false;
                characterLocomotion.enabled = false;
                thirdPersonController.enabled = false;
            }
        }
    }
}