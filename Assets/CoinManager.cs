using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("Player")){
            this.GetComponent<AudioSource>().Play();
            FindAnyObjectByType<CoinTracker>().updateCoins();
            this.gameObject.SetActive(false);
        }
    }
}
