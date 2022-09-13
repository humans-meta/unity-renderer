using FishNet.Object;
using UnityEngine;

/// <summary>
/// Contains information about the object on the network.
/// </summary>
public class FishnetNetworkInfo : NetworkBehaviour, INetworkInfo {
    /// <summary>
    /// Is the networking implementation server authoritative?
    /// </summary>
    /// <returns>True if the network transform is server authoritative.</returns>
    public bool IsServerAuthoritative() {
        return false;
    }

    /// <summary>
    /// Is the game instance on the server?
    /// </summary>
    /// <returns>True if the game instance is on the server.</returns>
    public bool IsServer() {
        return base.IsServer;
    }

    /// <summary>
    /// Is the character the local player?
    /// </summary>
    /// <returns>True if the character is the local player.</returns>
    public bool IsLocalPlayer() {
        return base.Owner.IsLocalClient;
    }
}

public interface INetworkInfo {
    public bool IsServerAuthoritative();
    public bool IsServer();
    bool IsLocalPlayer();
}