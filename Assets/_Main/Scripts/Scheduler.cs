using System;
using System.Collections;
using UnityEngine;

namespace _Main.Scripts {
    public class Scheduler : MonoBehaviour {
        public static Scheduler Instance;

        private void Awake() {
            Instance = this;
        }

        public static void Schedule(float delay, Action action) {
            Instance.StartCoroutine(Instance.Schedule_(delay, action));
        }

        IEnumerator Schedule_(float delay, Action action) {
            yield return new WaitForSeconds(delay);
            action();
        }
    }
}