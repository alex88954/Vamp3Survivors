using System;
using System.Collections.Generic;
using UnityEngine;
// Script pour le comportement de la boule de peur
public class BoulePeurScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ComportementPersonnage personnage = other.GetComponent<ComportementPersonnage>();
            personnage.OnboulePeurCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
