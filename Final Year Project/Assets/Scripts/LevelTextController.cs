using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelTextController : MonoBehaviour, IPointerClickHandler
{
    //holds the valid positions the text can be in
    //In order of: EARTH, MOON, MARS, JUPITER, SATURN, URANUS, NEPTUNE, PLUTO, VENUS
    private float[] validPositions = {27.6f, 16.7f,-0.6f,-21f,-47.73f,-72.4f,-93.1f,-111.6f,45.44f};
    private float currentPos;

    public Text thisText; //holds reference to this text object to make adjustments
    public GameController gameController; //holds reference to game controller to start game

    void Update () {
        //sets the position of the text to the correct planet based on the stage
        currentPos = validPositions[GameController.Stage-1];
        transform.position = new Vector3(currentPos, 42.5f, 0f);

        //changes the colour of the text based on the stage
        ChangeTextColor(GameController.Stage);

        //sets the text to display the level out of 5
        thisText.text = "" + GameController.Level + "/5";
    }

    
    private void ChangeTextColor(int stage)
    { //changes the colour of the text based on which stage it is
      //this is so that the text is visible no matter the colour of the planet
        switch (stage)
        {
            case 1:
            case 2:
            case 6:
            case 7:
            case 8:
                thisText.color = Color.red;
                break;
            case 3:
            case 4:
            case 5:
            case 9:
                thisText.color = Color.blue;
                break;
        }
    }
    
    public void OnPointerClick(PointerEventData eventData)
    { //the text will show on the current stage and so when clicked must start the game
        gameController.StartGame();
    }
}

