using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour {

    public Text shipWepLevel;
    public Text bossWepLevel;

    public Text shipWepPriceT;
    public Text bossWepPriceT;

    private int shipWepPrice;
    private int bossWepPrice;
    private int shipPartsPrice;
    private int bossPartsPrice;

    private int[] powerUpPrices;

    public Button buyShip;
    public Button buyBoss;
    public Button[] powerUps;

    // Use this for initialization
    void Start () {
        //initialises prices
		powerUpPrices = new int[] { 200, 150, 300 };
        shipWepPrice = 150 * GameManagerS.ShipWepLevel;
        bossWepPrice = 125 * GameManagerS.BossWepLevel;
        shipPartsPrice = 10 * GameManagerS.ShipWepLevel;
        bossPartsPrice = 10 * GameManagerS.BossWepLevel;
        //if the game starts with the player already on the max levels then don't show buy buts
        if(GameManagerS.ShipWepLevel == 3)
        {
            buyShip.gameObject.SetActive(false);
        }
        if(GameManagerS.BossWepLevel == 3)
        {
            buyBoss.gameObject.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        //update GUI text
        shipWepLevel.text = "Level " + GameManagerS.ShipWepLevel;
        bossWepLevel.text = "Level " + GameManagerS.BossWepLevel;

        shipWepPriceT.text = "£" + shipWepPrice + "+" + shipPartsPrice + " Parts";
        bossWepPriceT.text = "£" + bossWepPrice + "+" + bossPartsPrice + " Parts";

        //highlights powerup buttons according to static game manager list
        for(int i = 0; i < GameManagerS.PowerUps.Length; i++)
        {
            if (GameManagerS.PowerUps[i])
            {
                powerUps[i].GetComponent<Image>().color = Color.green;
            }
            else
            {
                powerUps[i].GetComponent<Image>().color = Color.red;
            }
        }
	}

    public void PowerUp(Button but)
    {
        //checks which button has been pressed and sets static bool to true at index
        if(but.gameObject.name == "Missiles")
        {
            //if player has enough money
            if (GameManagerS.Money >= powerUpPrices[0] && !PlayerController.MissileMode)
            {
                GameManagerS.PowerUps[0] = true;
                GameManagerS.Money -= powerUpPrices[0];
                PlayerController.MissileMode = true;
            }
        }
        else if(but.gameObject.name == "Shield")
        {
            //if player has enough money and is not already invulnerable
            if (GameManagerS.Money >= powerUpPrices[1] && !PlayerController.Invul)
            {
                GameManagerS.Money -= powerUpPrices[1];
                GameManagerS.PowerUps[1] = true;
                PlayerController.Invul = true;
            }
        }
        else
        {
            //if player has enough money
            if (GameManagerS.Money >= powerUpPrices[2])
            {
                GameManagerS.Money -= powerUpPrices[2];
                GameManagerS.PowerUps[2] = true;
            }
        }
    }

    public void Buy(Button but)
    {
        //buys different upgrade depending on the button clicked
        if(but.gameObject.name == "BuyShip")
        {
            //if player has enough money
            if(GameManagerS.Money >= shipWepPrice && GameManagerS.Parts >= shipPartsPrice)
            {
                //deduct money, increment wep level, make the price higher for the next lvl and change damage
                GameManagerS.Money -= shipWepPrice;
                GameManagerS.Parts -= shipPartsPrice;
                GameManagerS.ShipWepLevel++;
                shipWepPrice = 150 * GameManagerS.ShipWepLevel;
                shipPartsPrice = 10 * GameManagerS.ShipWepLevel;
                PlayerController.Damage = 25 * GameManagerS.ShipWepLevel;
                //if the level is now 3, don't show the buy button anymore
                if(GameManagerS.ShipWepLevel == 3)
                {
                    but.gameObject.SetActive(false);
                }
            }
        }
        //ditto for buy boss button
        else if (but.gameObject.name == "BuyBoss")
        {
            if (GameManagerS.Money >= bossWepPrice && GameManagerS.Parts >= bossPartsPrice)
            {
                GameManagerS.Money -= bossWepPrice;
                GameManagerS.Parts -= bossPartsPrice;
                GameManagerS.BossWepLevel++;
                bossWepPrice = 125 * GameManagerS.BossWepLevel;
                bossPartsPrice = 10 * GameManagerS.BossWepLevel;
                CharController.Damage = 40 * GameManagerS.BossWepLevel;
                if (GameManagerS.BossWepLevel == 3)
                {
                    but.gameObject.SetActive(false);
                }
            }
        }
    }
}
