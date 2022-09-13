using System.Collections;
using System.Collections.Generic;
using _Main.Scripts;
using FishNet.Object;
using UnityEngine;

public class FishnetNetworkCharacter : NetworkBehaviour, INetworkCharacter {
    public void LoadDefaultLoadout() {
        throw new System.NotImplementedException();
    }

    public void EquipUnequipItem(uint itemIdentifierID, int slotID, bool equip) {
        throw new System.NotImplementedException();
    }

    public void ItemIdentifierPickup(uint itemIdentifierID, int amount, int slotID, bool immediatePickup,
                                     bool forceEquip) {
        throw new System.NotImplementedException();
    }

    public void RemoveAllItems() {
        throw new System.NotImplementedException();
    }

    public void Fire(ItemAction itemAction, float strength) {
        throw new System.NotImplementedException();
    }

    public void StartItemReload(ItemAction itemAction) {
        throw new System.NotImplementedException();
    }

    public void ReloadItem(ItemAction itemAction, bool fullClip) {
        throw new System.NotImplementedException();
    }

    public void ItemReloadComplete(ItemAction itemAction, bool success, bool immediateReload) {
        throw new System.NotImplementedException();
    }

    public void MeleeHitCollider(ItemAction itemAction, int hitboxIndex, RaycastHit raycastHit,
                                 GameObject hitGameObject,
                                 UltimateCharacterLocomotion hitCharacterLocomotion) {
        throw new System.NotImplementedException();
    }

    public void ThrowItem(ItemAction itemAction) {
        throw new System.NotImplementedException();
    }

    public void EnableThrowableObjectMeshRenderers(ItemAction itemAction) {
        throw new System.NotImplementedException();
    }

    public void StartStopBeginEndMagicActions(ItemAction itemAction, bool beginActions, bool start) {
        throw new System.NotImplementedException();
    }

    public void MagicCast(ItemAction itemAction, int index, uint castID, Vector3 direction, Vector3 targetPosition) {
        throw new System.NotImplementedException();
    }

    public void MagicImpact(ItemAction itemAction, uint castID, GameObject source, GameObject target, Vector3 position,
                            Vector3 normal) {
        throw new System.NotImplementedException();
    }

    public void StopMagicCast(ItemAction itemAction, int index, uint castID) {
        throw new System.NotImplementedException();
    }

    public void ToggleFlashlight(ItemAction itemAction, bool active) {
        throw new System.NotImplementedException();
    }

    public void PushRigidbody(Rigidbody targetRigidbody, Vector3 force, Vector3 point) {
        throw new System.NotImplementedException();
    }

    public void SetRotation(Quaternion rotation, bool snapAnimator) {
        throw new System.NotImplementedException();
    }

    public void SetPosition(Vector3 position, bool snapAnimator) {
        throw new System.NotImplementedException();
    }

    public void ResetRotationPosition() {
        // throw new System.NotImplementedException();
    }

    public void SetPositionAndRotation(Vector3 position, Quaternion rotation, bool snapAnimator,
                                       bool stopAllAbilities) {
        // throw new System.NotImplementedException();
    }

    public void SetActive(bool active, bool uiEvent) {
        throw new System.NotImplementedException();
    }
}

public class ItemAction {
}

public interface INetworkCharacter {
}