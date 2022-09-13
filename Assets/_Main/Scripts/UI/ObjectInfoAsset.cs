using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Meta/Object Info")]
public class ObjectInfoAsset : ScriptableObject {
    [TextArea(3, 20)] public string text;
}