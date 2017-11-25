using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FireZoneScript : MonoBehaviour, IPointerDownHandler, IPointerExitHandler {
    public bool canFire;
    // Use this for initialization
    public void OnPointerDown(PointerEventData eventData)
    {
        canFire = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        canFire = false;
    }
}
