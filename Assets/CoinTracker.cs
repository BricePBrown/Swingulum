using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinTracker : MonoBehaviour
{
    [HideInInspector]
    public int currentCoins;
    public int maxCoins;

    public TextMeshPro text;

    private void Start()
    {
        text = this.GetComponent<TextMeshPro>();
        maxCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void updateCoins()
    {
        currentCoins++;
        text.text = currentCoins + " / " + maxCoins + " Coins";
    }
}
