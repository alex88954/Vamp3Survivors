using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class CodesTriche : MonoBehaviour
{
    private GestionnaireJeu gestionnaireJeu;
    private GestionnaireAmeliorations gestionnaireAmeliorations;
    [SerializeField] private GameObject experiencePrefab;
    [SerializeField] private GameObject bouleViePrefab;
    [SerializeField] private GameObject boulePeurPrefab;

    void Start()
    {
        gestionnaireJeu = GetComponent<GestionnaireJeu>();
        gestionnaireAmeliorations = GetComponent<GestionnaireAmeliorations>();
    }

    void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            gestionnaireJeu.Joueur.RedonnerMaxVies();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            Instantiate(experiencePrefab, InstantiateUtilPos(), transform.rotation);
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            gestionnaireJeu.Joueur.GagnerExperience(5);
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            gestionnaireJeu.Joueur.GagnerExperience(100);
        }

        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            gestionnaireJeu.Joueur.PerdreVie(1);
        }
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            gestionnaireJeu.Joueur.PerdreVie(999);
        }

        if (Keyboard.current.digit0Key.wasPressedThisFrame)
        {
            foreach (var ennemi in FindObjectsByType<ComportementEnnemi>(FindObjectsSortMode.None))
            {
                Destroy(ennemi.gameObject);
            }
        }
        // Ajouter une vague d'ennemis
        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            gestionnaireJeu.AjouterVagueEnnemi();
        }
        // Ajouter une boule de vie
        if (Keyboard.current.digit6Key.wasPressedThisFrame)
        {
            Instantiate(bouleViePrefab, InstantiateUtilPos(), transform.rotation);
        }
        // Ajouter une boule de peur
        if (Keyboard.current.digit7Key.wasPressedThisFrame)
        {
            Instantiate(boulePeurPrefab, InstantiateUtilPos(), transform.rotation);
        }
        // Ajouter de l'argent
        if (Keyboard.current.digit8Key.wasPressedThisFrame)
        {
            gestionnaireJeu.Joueur.GagnerArgent(50);
        }
        // Débloquer les améliorations de boule de feu et de livre
        if (Keyboard.current.digit9Key.wasPressedThisFrame)
        {
            gestionnaireAmeliorations.DebloquerBouleFeu();
            gestionnaireAmeliorations.DebloquerLivre();
        }
        // Faire un ennemi apparaitre
        if (Keyboard.current.gKey.wasPressedThisFrame)
        {
            gestionnaireJeu.ApparaitreEnnemi(false);
        }
        // Faire un ennemi apparaitre en sautant
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            gestionnaireJeu.ApparaitreEnnemi(true);
        }
    }
    // Donner une position pour instantiate autour Olivia
    private Vector3 InstantiateUtilPos()
    {
        var centre = gestionnaireJeu.Joueur.transform.position;
        return centre + new Vector3(
           Random.value * 5 - 2.5f,
           0,
           Random.value * 5 - 2.5f);
    }
}