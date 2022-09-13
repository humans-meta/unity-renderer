using System.Collections;
using System.Collections.Generic;
using FishNet.Managing.Transporting;
using FishNet.Transporting.Bayou;
using FishNet.Transporting.Multipass;
using FishNet.Transporting.Tugboat;
using UnityEngine;

public class FishnetTransportController : MonoBehaviour {
    void Awake() {
        var transportManager = GetComponent<TransportManager>();
        var mp = (Multipass)transportManager.GetTransport<Multipass>();

#if (UNITY_WEBGL && !UNITY_EDITOR) || UNITY_SERVER
        mp.SetClientTransport<Bayou_ReverseProxy>();
#else
        mp.SetClientTransport<Tugboat>();
#endif
    }
}
