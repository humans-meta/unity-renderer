using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishnetNetworkRespawnerMonitor : MonoBehaviour, INetworkRespawnerMonitor {
    public void Respawn(Vector3 position, Quaternion rotation, bool transformChange) {
        // ignore
    }
}

public interface INetworkRespawnerMonitor {
}