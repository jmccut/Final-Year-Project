using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireZoneScript : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {
    public bool canFire;

    //if touching left side of screen, set flag to true
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
