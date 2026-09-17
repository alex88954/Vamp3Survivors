using UnityEngine;
// Script pour le comportement de la boule de peur
public class BouleVieScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ComportementPersonnage personnage = other.GetComponent<ComportementPersonnage>();
            personnage.RedonnerMaxVies();
            Destroy(gameObject);
        }
    }
}
