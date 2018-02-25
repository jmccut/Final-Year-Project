using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PBButtonHandler : MonoBehaviour, IPointerClickHandler
{
    private int BasePrice;
    private int damage;
    private int health;
    private int maxHealth;
    public Slider healthBar;
    public Text levelText;
    private bool dead;
    public ChangeScene change;
    //assigns the button
    private bool isUpgrade;
    private bool isRepair;
    private int parentName;
    void Start()
    {
        //sets flag depending on which button this script is attached to
        isUpgrade = gameObject.name == "upgrade";
        isRepair = gameObject.name == "repair";
        //sets base price of an upgrade to a planet base level
        BasePrice = 120;
        maxHealth = 100;
        //to identify the planet index to be upgraded, im using the names which i have
        //numbered from 1-9
        parentName = int.Parse(transform.parent.name) - 1;
        health = maxHealth - GameManagerS.BaseDamage[parentName];
        healthBar.value = health / maxHealth;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //handle interaction for different buttons
        if (isUpgrade)
        {
            Upgrade(parentName);
            
        }
        else if(isRepair)
        {
            Repair(parentName);
        }

    }

    public void Update()
    {
        //if the stage has not been reached yet, grey it out
        if (GameManagerS.Stage > parentName+1)
        {
            transform.parent.GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            transform.parent.GetComponent<SpriteRenderer>().color = Color.grey;
        }
        //update level text and health bar
        levelText.text = "Level : " + GameManagerS.BaseLevels[parentName];
        health = maxHealth - GameManagerS.BaseDamage[parentName];
        healthBar.value = health / maxHealth;
        if (health <= 0)
        {
            Dead();
        }
    }

    public void Repair(int index)
    {
        //if the player has 5*damage money and there is actually damage 
        //and they have unlocked the planet
        if (GameManagerS.Money >= 5 * (GameManagerS.BaseDamage[index] + 1)
            && GameManagerS.BaseDamage[index] != 0
            && GameManagerS.Stage > index+1)
        {
            //take away money
            GameManagerS.Money -= 5 * (GameManagerS.BaseDamage[index] + 1);
            //reset their damage
            GameManagerS.BaseDamage[index] = 0;
            //sets objective to true
            GameManagerS.CompleteObjList[8] = true;
            if (dead)
            {
                //the player has clicked on an invaded planet
                GameManagerS.CurrentPlanet = parentName;
                change.Change(6);
            }
        }
        //if the base is dead but they don't have enough money
        else if (5 * (GameManagerS.BaseDamage[index] + 1) > GameManagerS.Money 
            && dead && GameManagerS.BaseDamage[index] != 0 && GameManagerS.Stage > index + 1)
        {
            GameManagerS.BaseDamage[index] = 0;
            GameManagerS.Money = 0;
            GameManagerS.CurrentPlanet = parentName;
            change.Change(6);
        }
    }

    public void Upgrade(int index)
    {
        //if the player has enough money for the upgrade
        //which is the base price of 100 multiplied by the current level of the base
        //and if the base has not already reached its max level which is 3
        if (GameManagerS.Money >= BasePrice * (GameManagerS.BaseLevels[index] + 1)
            && GameManagerS.BaseLevels[index] != 3
            && GameManagerS.Stage > index+1
            && !dead)
        {
            //take away money
            GameManagerS.Money -= BasePrice * GameManagerS.BaseLevels[index];
            //level is incremented in saved array
            GameManagerS.BaseLevels[index]++;
            //if first base is built
            if (GameManagerS.BaseLevels[index] == 1)
            {
                //set base sprite to active
                transform.parent.transform.GetChild(0).gameObject.SetActive(true);
            }
        }
    }

    public void Dead()
    {
        //destroy the base
        transform.parent.transform.GetChild(0).gameObject.SetActive(false);
        //turn the planet red
        transform.parent.GetComponent<SpriteRenderer>().color = Color.red;
        dead = true;
    }
}
