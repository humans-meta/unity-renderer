using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using EventHandler = _Main.Scripts.EventHandler;

public class BlockchainController : MonoBehaviour {
    public Transform nodes;
    public float moveDuration = 2f;
    public float makeDuration = 3f;
    public Animator rotorAnimator;

    private List<Transform> _nodes = new();
    private Sequence _seq;

    private void Awake() {
        foreach (Transform child in nodes.transform) {
            _nodes.Add(child);
        }

        EventHandler.RegisterEvent<World>("OnWorldWillChange", OnWorldWillChange);
    }

    private void OnDestroy() {
        EventHandler.UnregisterEvent<World>("OnWorldWillChange", OnWorldWillChange);
    }

    private void OnWorldWillChange(World world) {
        if (world.GetComponent<BlockchainInteriorController>() != null) {
            _seq.Pause();
            if (rotorAnimator != null)
                rotorAnimator.enabled = false;
        }
        else {
            _seq.Play();
            if (rotorAnimator != null)
                rotorAnimator.enabled = true;
        }
    }

    private void Start() {
        _seq = DOTween.Sequence(this);

        foreach (var node in _nodes) {
            var anchor = node.Find("Anchor");
            var offset = anchor.localPosition.y;
            _seq.Join(node.DOLocalMoveY(offset, moveDuration)
                          .SetRelative(true));
        }

        var lastNode = _nodes.Last();
        _seq.Join(lastNode.DOScale(0, moveDuration));

        _seq.Append(lastNode.DOLocalMoveY(0, 0))
            .Append(lastNode.DOScale(1, makeDuration));

        _seq.SetLoops(-1);
    }
}