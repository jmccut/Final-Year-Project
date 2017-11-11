using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapController : MonoBehaviour, IPointerClickHandler {
    public GameController gameController;
public void OnPointerClick(PointerEventData eventData)
    {
        gameController.StartGame();
    }
}
