﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GadgetShopManager : MonoBehaviour {

    private Button[] buttons;
    private Image[] images;
    private Text[] nameAndPrice;
    private DataManager dataManager;
    public GameObject[] gadgets;
    public Text description;
    public GameObject buyButton;
    private GadgetBuyer gadgetBuyer;
    private int currentGadget = -1;
    public Text yourMoney;



    // Use this for initialization
    void Start () {
        dataManager = GameObject.FindGameObjectWithTag("DataManager").GetComponent<DataManager>();
        buttons = new Button[dataManager.Gadgets.Length];
        images = new Image[dataManager.Gadgets.Length];
        nameAndPrice = new Text[dataManager.Gadgets.Length];
        gadgetBuyer = buyButton.GetComponent<GadgetBuyer>();


        for (int i = 0; i < dataManager.Gadgets.Length; i++)
        {
            gadgets[i].SetActive(true);
            buttons[i] = gadgets[i].GetComponentInChildren<Button>();
            images[i] = gadgets[i].transform.GetChild(0).GetComponent<Image>();
            nameAndPrice[i] = gadgets[i].GetComponentInChildren<Text>();
            images[i].sprite = dataManager.Gadgets[i].image;
            nameAndPrice[i].text = dataManager.Gadgets[i].gadgetName + "\n" + dataManager.Gadgets[i].price + " $";
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        

        for(int i = 0; i < dataManager.Gadgets.Length; i++)
        {
            if (!dataManager.Gadgets[i].isLocked)
            {
                buttons[i].interactable = false;
                images[i].color = new Color(0.2f, 0.2f, 0.2f);
            }
        }

        updateDescription();

        yourMoney.text = "Money: " + dataManager.MoneyData + "$";
    }

    private void updateDescription()
    {
        if (currentGadget != gadgetBuyer.getGadget())
        {
            currentGadget = gadgetBuyer.getGadget();
            description.text = dataManager.Gadgets[currentGadget].description;
        }
    }

    
}
