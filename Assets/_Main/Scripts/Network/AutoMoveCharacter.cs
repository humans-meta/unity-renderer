using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class AutoMoveCharacter : MonoBehaviour {
    public float zMax = 5f;

    private Keyboard keyboard;

    private Vector2 goForwardState;

    private Vector2 goBackState;

    private Vector2 currentState;
    // private KeyboardState goForwardState;
    // private KeyboardState goBackState;
    // private KeyboardState currentState;

    private void Start() {
        // keyboard = InputSystem.GetDevice<Keyboard>();
        //
        // goForwardState = new KeyboardState();
        // goForwardState.Press(Key.W);
        //
        // goBackState = new KeyboardState();
        // goBackState.Press(Key.S);

        goForwardState = new Vector2(0, 1f);
        goBackState = new Vector2(0, -1f);

        currentState = goForwardState;
    }

    void Update() {
        var localPlayer = WorldManager.Instance.LocalPlayer;
        if (localPlayer != null) {
            var input = localPlayer.GetComponent<StarterAssetsInputs>();
            var position = localPlayer.position;
            if (position.z > zMax) {
                currentState = goBackState;
            }
            else if (position.z < 0) {
                currentState = goForwardState;
            }

            input.MoveInput(currentState);
            // InputSystem.QueueStateEvent(keyboard, currentState);
        }
    }
}