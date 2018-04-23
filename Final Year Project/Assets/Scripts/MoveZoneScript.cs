using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveZoneScript : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {
    public bool canMove; //flag to indicate area is being touched

    //when touch starts
    public void OnPointerDown(PointerEventData eventData)
    {
        canMove = true;
    }
    //when touch ends
    public void OnPointerExit(PointerEventData eventData)
    {
        canMove = false;
    }
}
