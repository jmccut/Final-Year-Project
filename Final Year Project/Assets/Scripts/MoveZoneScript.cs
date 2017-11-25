using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveZoneScript : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {
    public bool canMove;
    public void OnPointerDown(PointerEventData eventData)
    {
        canMove = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canMove = false;
    }
}
