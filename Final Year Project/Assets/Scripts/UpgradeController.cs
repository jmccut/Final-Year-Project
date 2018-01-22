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

    private int[] powerUpPrices;

    public Button buyShip;
    public Button buyBoss;

    // Use this for initialization
    void Start () {
        //initialises prices
		powerUpPrices = new int[] { 100, 200, 300 };
        shipWepPrice = 150 * GameManagerS.ShipWepLevel;
        bossWepPrice = 125 * GameManagerS.BossWepLevel;
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

        shipWepPriceT.text = "£" + shipWepPrice;
        bossWepPriceT.text = "£" + bossWepPrice;
	}

    public void Highlight(Button but)
    {
        //used to highlight button when clicked 
        but.GetComponent<Image>().color = Color.green;
    }

    public void Buy(Button but)
    {
        //buys different upgrade depending on the button clicked
        if(but.gameObject.name == "BuyShip")
        {
            //if player has enough money
            if(GameManagerS.Money >= shipWepPrice)
            {
                //deduct money, increment wep level, make the price higher for the next lvl and change damage
                GameManagerS.Money -= shipWepPrice;
                GameManagerS.ShipWepLevel++;
                shipWepPrice = 150 * GameManagerS.ShipWepLevel;
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
            if (GameManagerS.Money >= bossWepPrice)
            {
                GameManagerS.Money -= bossWepPrice;
                GameManagerS.BossWepLevel++;
                bossWepPrice = 125 * GameManagerS.BossWepLevel;
                CharController.Damage = 40 * GameManagerS.BossWepLevel;
                if (GameManagerS.BossWepLevel == 3)
                {
                    but.gameObject.SetActive(false);
                }
            }
        }
    }
}
