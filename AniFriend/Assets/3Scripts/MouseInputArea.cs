using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;

public sealed class MouseInputArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    CinemachineFreeLook cine;

    void Start() {
        cine = FindObjectOfType<CinemachineFreeLook>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (cine != null) {
            cine.m_XAxis.m_InputAxisName = "Mouse X";
            cine.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if (cine != null) {
            cine.m_XAxis.m_InputAxisName = "";
            cine.m_YAxis.m_InputAxisName = "";
        }
    }
}
