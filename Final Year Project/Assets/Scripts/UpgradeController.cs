using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeController : MonoBehaviour {
    //upgrade text
    public Text shipWepLevel;
    public Text bossWepLevel;
    public Text shipWepPriceT;
    public Text bossWepPriceT;
    //prices for upgrades
    private int shipWepPrice;
    private int bossWepPrice;
    private int shipPartsPrice;
    private int bossPartsPrice;
    private int[] powerUpPrices;
    //upgrade buttons
    public Button buyShip;
    public Button buyBoss;
    public Button[] powerUps;

    void Start () {
        //initialises prices
		powerUpPrices = new int[] { 250, 200, 150 };
        //prices of upgrades scale by the current level
        shipWepPrice = 150 * GameManagerS.ShipWepLevel;
        bossWepPrice = 125 * GameManagerS.BossWepLevel;
        shipPartsPrice = 10 * GameManagerS.ShipWepLevel;
        bossPartsPrice = 10 * GameManagerS.BossWepLevel;

        //if the game starts with the player already on the max level
        //of upgrade then don't show the upgrade buttons
        if(GameManagerS.ShipWepLevel == 3)
        {
            buyShip.gameObject.SetActive(false);
        }
        if(GameManagerS.BossWepLevel == 3)
        {
            buyBoss.gameObject.SetActive(false);
        }
	}
	
    //Update is called once per frame
	void Update () {
        //update text to show upgrade levels & prices
        shipWepLevel.text = "Level " + GameManagerS.ShipWepLevel;
        bossWepLevel.text = "Level " + GameManagerS.BossWepLevel;

        shipWepPriceT.text = "£" + shipWepPrice + "+" + shipPartsPrice + " Parts";
        bossWepPriceT.text = "£" + bossWepPrice + "+" + bossPartsPrice + " Parts";

        //highlights active powerup buttons green according to static game manager list
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
                //activate power-up flags and decrement money
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
                PBPlayerController.Invul = true;
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
            //if player has enough money and weapon parts
            if(GameManagerS.Money >= shipWepPrice && GameManagerS.Parts >= shipPartsPrice)
            {
                //deduct money/parts, increment wep level, make the price higher for the next lvl and change damage
                GameManagerS.Money -= shipWepPrice;
                GameManagerS.Parts -= shipPartsPrice;
                GameManagerS.ShipWepLevel++;
                shipWepPrice = 150 * GameManagerS.ShipWepLevel;
                shipPartsPrice = 10 * GameManagerS.ShipWepLevel;
                PlayerController.Damage = 25 * GameManagerS.ShipWepLevel;

                //if the level of the weapon is now 3, don't show the buy button anymore
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
