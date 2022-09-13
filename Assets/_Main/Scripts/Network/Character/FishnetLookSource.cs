using _Main.Scripts;
using FishNet.Object;
using UnityEngine;

/// <summary>
/// Syncronizes the ILookSource over the network.
/// </summary>
public class FishnetLookSource : NetworkBehaviour, ILookSource {
    [Tooltip("A multiplier to apply to the networked values for remote players.")] [SerializeField]
    protected float m_RemoteInterpolationMultiplayer = 1.2f;

    private GameObject m_GameObject;
    private Transform m_Transform;
    private UltimateCharacterLocomotion m_CharacterLocomotion;
    private ILookSource m_LookSource;

    public GameObject GameObject {
        get { return m_GameObject; }
    }

    public Transform Transform {
        get { return m_Transform; }
    }

    public float LookDirectionDistance {
        get { return m_NetworkLookDirectionDistance; }
    }

    public float Pitch {
        get { return m_NetworkPitch; }
    }

    private float m_NetworkLookDirectionDistance = 1;
    private float m_NetworkTargetLookDirectionDistance = 1;
    private float m_NetworkPitch;
    private float m_NetworkTargetPitch;
    private Vector3 m_NetworkLookPosition;
    private Vector3 m_NetworkTargetLookPosition;
    private Vector3 m_NetworkLookDirection;
    private Vector3 m_NetworkTargetLookDirection;

    private bool m_InitialSync = true;

    /// <summary>
    /// Specifies which look source objects are dirty.
    /// </summary>
    private enum TransformDirtyFlags : byte {
        LookDirectionDistance = 1, // The Look Direction Distance has changed.
        Pitch = 2, // The Pitch has changed.
        LookPosition = 4, // The Look Position has changed.
        LookDirection = 8, // The Look Direction has changed.
    }

    /// <summary>
    /// Initialize the default values.
    /// </summary>
    private void Awake() {
        m_GameObject = gameObject;
        m_Transform = transform;
        m_CharacterLocomotion = m_GameObject.GetComponent<UltimateCharacterLocomotion>();

        m_NetworkLookPosition = m_NetworkTargetLookPosition = m_Transform.position;
        m_NetworkLookDirection = m_NetworkTargetLookDirection = m_Transform.forward;

        EventHandler.RegisterEvent<ILookSource>(m_GameObject, "OnCharacterAttachLookSource", OnAttachLookSource);
    }

    /// <summary>
    /// Register for any interested events.
    /// </summary>
    public override void OnStartNetwork() {
        base.OnStartNetwork();
        
        // Remote characters will not have a local look source. The current component should act as the look source.
        if (!base.Owner.IsLocalClient) {
            EventHandler.UnregisterEvent<ILookSource>(m_GameObject, "OnCharacterAttachLookSource", OnAttachLookSource);
            EventHandler.ExecuteEvent<ILookSource>(m_GameObject, "OnCharacterAttachLookSource", this);
        }
    }

    /// <summary>
    /// A new ILookSource object has been attached to the character.
    /// </summary>
    /// <param name="lookSource">The ILookSource object attached to the character.</param>
    private void OnAttachLookSource(ILookSource lookSource) {
        m_LookSource = lookSource;
    }

    /// <summary>
    /// Returns the position of the look source.
    /// </summary>
    /// <param name="characterLookPosition">Is the character look position being retrieved?</param>
    /// <returns>The position of the look source.</returns>
    public Vector3 LookPosition(bool characterLookPosition) {
        return m_NetworkLookPosition;
    }

    /// <summary>
    /// Returns the direction that the character is looking.
    /// </summary>
    /// <param name="characterLookDirection">Is the character look direction being retrieved?</param>
    /// <returns>The direction that the character is looking.</returns>
    public Vector3 LookDirection(bool characterLookDirection) {
        if (characterLookDirection) {
            return m_Transform.forward;
        }

        return m_NetworkLookDirection;
    }

    public Vector3 LookDirection(Vector3 lookPosition, bool characterLookDirection, int layerMask, bool includeRecoil,
                                 bool includeMovementSpread) {
        // var collisionLayerEnabled = m_CharacterLocomotion.CollisionLayerEnabled;
        // m_CharacterLocomotion.EnableColliderCollisionLayer(false);

        // Cast a ray from the look source point in the forward direction. The look direction is then the vector from the look position to the hit point.
        RaycastHit hit;
        Vector3 direction;
        if (Physics.Raycast(m_NetworkLookPosition, m_NetworkLookDirection, out hit, m_NetworkLookDirectionDistance,
                            layerMask, QueryTriggerInteraction.Ignore)) {
            direction = (hit.point - lookPosition).normalized;
        }
        else {
            direction = m_NetworkLookDirection;
        }

        // m_CharacterLocomotion.EnableColliderCollisionLayer(collisionLayerEnabled);
        return direction;
    }

    private void Update() {
        // Local players do not need to interpolate the look values.
        if (base.IsOwner) {
            return;
        }

        var serializationRate = (1f / 1f) * m_RemoteInterpolationMultiplayer;
        m_NetworkLookDirectionDistance = Mathf.MoveTowards(m_NetworkLookDirectionDistance,
                                                           m_NetworkTargetLookDirectionDistance,
                                                           Mathf.Abs(m_NetworkTargetLookDirectionDistance -
                                                                     m_NetworkLookDirectionDistance) *
                                                           serializationRate);
        m_NetworkPitch = Mathf.MoveTowards(m_NetworkPitch, m_NetworkTargetPitch,
                                           Mathf.Abs(m_NetworkTargetPitch - m_NetworkPitch) * serializationRate);
        m_NetworkLookPosition = Vector3.MoveTowards(m_NetworkLookPosition, m_NetworkTargetLookPosition,
                                                    (m_NetworkTargetLookPosition - m_NetworkLookPosition).magnitude *
                                                    serializationRate);
        m_NetworkLookDirection = Vector3.MoveTowards(m_NetworkLookDirection, m_NetworkTargetLookDirection,
                                                     (m_NetworkTargetLookDirection - m_NetworkLookDirection).magnitude *
                                                     serializationRate);
    }

    private void OnDestroy() {
        EventHandler.UnregisterEvent<ILookSource>(m_GameObject, "OnCharacterAttachLookSource", OnAttachLookSource);
    }
}

public interface ILookSource {
}