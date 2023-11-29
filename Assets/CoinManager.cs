using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    private bool hasPlayedSound = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayedSound && other.gameObject.name.Equals("Player"))
        {
            // Play the sound
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Play();
            }

            // Move the coin up by 100 units
            transform.Translate(Vector3.up * 100f);

            // Set a flag to prevent playing the sound again
            hasPlayedSound = true;

            // Deactivate the GameObject after the sound finishes (adjust the delay as needed)
            float soundDuration = audioSource.clip.length;
            Invoke("DeactivateCoin", soundDuration);
        }
    }

    private void DeactivateCoin()
    {
        gameObject.SetActive(false);
    }
}
