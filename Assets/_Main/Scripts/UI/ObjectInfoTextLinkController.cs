using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ObjectInfoTextLinkController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public RectTransform TextPopup_Prefab_01;
    public RectTransform rootContainer;

    private TextMeshProUGUI m_TextMeshPro;
    private Canvas m_Canvas;
    private Camera m_Camera;
    private RectTransform m_TextPopup_RectTransform;
    private TextMeshProUGUI m_TextPopup_TMPComponent;

    private int m_selectedLink = -1;
    private bool isHoveringObject;
    private Dictionary<string, ObjectInfoAsset> db = new();

    void Awake() {
        m_TextMeshPro = gameObject.GetComponent<TextMeshProUGUI>();

        m_Canvas = FindObjectOfType<Canvas>();

        // Get a reference to the camera if Canvas Render Mode is not ScreenSpace Overlay.
        if (m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            m_Camera = null;
        else
            m_Camera = m_Canvas.worldCamera;

        // Create pop-up text object which is used to show the link information.
        m_TextPopup_RectTransform = Instantiate(TextPopup_Prefab_01, m_Canvas.transform, false) as RectTransform;
        m_TextPopup_TMPComponent = m_TextPopup_RectTransform.GetComponentInChildren<TextMeshProUGUI>();
        m_TextPopup_RectTransform.gameObject.SetActive(false);

        var resources = Resources.LoadAll<ObjectInfoAsset>("ObjectInfo");
        foreach (var r in resources) {
            db.Add(r.name, r);
        }
    }

    private void LateUpdate() {
        if (isHoveringObject) {
            // Check if mouse intersects with any links.
            int linkIndex = TMP_TextUtilities.FindIntersectingLink(m_TextMeshPro, Input.mousePosition, m_Camera);

            // Clear previous link selection if one existed.
            if ((linkIndex == -1 && m_selectedLink != -1) || linkIndex != m_selectedLink) {
                m_TextPopup_RectTransform.gameObject.SetActive(false);
                m_selectedLink = -1;
            }

            // Handle new Link selection.
            if (linkIndex != -1 && linkIndex != m_selectedLink) {
                m_selectedLink = linkIndex;
                TMP_LinkInfo linkInfo = m_TextMeshPro.textInfo.linkInfo[linkIndex];
                if (db.TryGetValue(linkInfo.GetLinkID(), out var objectInfo)) {
                    RectTransformUtility.ScreenPointToWorldPointInRectangle(
                        m_TextMeshPro.rectTransform, Input.mousePosition, m_Camera, out var worldPointInRectangle);
                    var corners = new Vector3[4];
                    rootContainer.GetWorldCorners(corners);
                    m_TextPopup_RectTransform.position =
                        new Vector3(corners[2].x + 8, worldPointInRectangle.y, worldPointInRectangle.z);
                    m_TextPopup_TMPComponent.text = objectInfo.text;
                    m_TextPopup_TMPComponent.ForceMeshUpdate(true, true);
                    var summary = m_TextPopup_TMPComponent.textInfo.linkInfo
                        .First(info => info.GetLinkID() == "_summary");
                    // var summaryText = m_TextPopup_TMPComponent.text.Substring(summary.linkTextfirstCharacterIndex,
                    //                                                           summary.linkTextLength);
                    m_TextPopup_TMPComponent.text = summary.GetLinkText();
                    LayoutRebuilder.ForceRebuildLayoutImmediate(m_TextPopup_RectTransform);
                    LayoutRebuilder.ForceRebuildLayoutImmediate(m_TextPopup_RectTransform);
                    m_TextPopup_RectTransform.gameObject.SetActive(true);
                }
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        isHoveringObject = true;
    }

    public void OnPointerExit(PointerEventData eventData) {
        isHoveringObject = false;
    }
}