using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class WorldTransition : MonoBehaviour {
    // public AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
    public float duration = 0.5f;

    public void StartTransition(Transform tf, Vector3 toPosition, Quaternion toRotation, Vector3 toScale,
                                Action onFinish) {
        DOTween.Sequence()
            .Join(tf.DOMove(toPosition, duration))
            .Join(tf.DORotateQuaternion(toRotation, duration))
            .Join(tf.DOScale(toScale, duration))
            .AppendCallback(() => onFinish());
    }
}