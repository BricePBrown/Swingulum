using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        text.text = currentCoins + " / " + maxCoins + " Coins";
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "CoinsMax", maxCoins);
    }

    public void updateCoins()
    {
        currentCoins++;
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "CoinsGet", currentCoins);
        text.text = currentCoins + " / " + maxCoins + " Coins";
    }
}
