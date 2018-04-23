using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireZoneScript : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {
    public bool canFire; //flag to indicate if player can fire

    //if touching left side of screen
    public void OnPointerDown(PointerEventData eventData)
    {
        canFire = true;
    }

    //when finger lifted, reset flag
    public void OnPointerExit(PointerEventData eventData)
    {
        canFire = false;
    }
}
