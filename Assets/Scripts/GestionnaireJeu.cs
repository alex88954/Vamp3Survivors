using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class GestionnaireJeu : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabEnnemis;

    [SerializeField] private ComportementPersonnage joueur;
    public ComportementPersonnage Joueur => joueur;

    [SerializeField] private TMP_Text txtCompteurCreaturesLibres;
    [SerializeField] private TMP_Text txtCompteurCreaturesContenues;
    [SerializeField] private TMP_Text txtCompteurFin;

    [SerializeField] private GameObject ecranFin;
    [SerializeField] private RectTransform imageGlitchTransform;
    private StrategieApparitionVague[] strategiesApparitionVague;
    private HashSet<ComportementEnnemi> tousEnnemis = new HashSet<ComportementEnnemi>();

    public event System.Action<int> OnJeuTermine;


    private int nbCreaturesLibres = 0;
    private int nbCreaturesContenues = 0;
    [SerializeField] private float tempsEntreVagues = 15f;
    int nbEnnemisParVague;

    private Coroutine ajouterEnnemis1, ajouterEnnemis2;

    void Start()
    {
        nbEnnemisParVague = ParametresJeu.Instance.EnnemisParVague;

        txtCompteurCreaturesLibres.text = "0";
        txtCompteurCreaturesContenues.text = "0";

        ajouterEnnemis1 = StartCoroutine(AjouterEnnemisPeriodiquement());
        ajouterEnnemis2 = StartCoroutine(AjouterVagueEnnemisPeriodiquement());

        joueur.OnMort += FinDuJeu;
        joueur.OnboulePeurCollected += TerroriserMonstres;

        strategiesApparitionVague = ParametresJeu.Instance.strategiesApparitionVague;
    }

    private void FinDuJeu()
    {
        txtCompteurFin.text = nbCreaturesContenues + "";
        ecranFin.SetActive(true);
        StartCoroutine(AnimationFin());
        StartCoroutine(DelaiRetourMenu());

        StopCoroutine(ajouterEnnemis1);
        StopCoroutine(ajouterEnnemis2);
        foreach (var ennemi in FindObjectsByType<ComportementEnnemi>(FindObjectsSortMode.None))
            Destroy(ennemi);
        OnJeuTermine?.Invoke(nbCreaturesContenues);
    }

    private IEnumerator AnimationFin()
    {
        while (true)
        {
            imageGlitchTransform.anchoredPosition = new Vector2(Random.Range(-960f, 960f), 0);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private IEnumerator DelaiRetourMenu()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator AjouterEnnemisPeriodiquement()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            ApparaitreEnnemi(false);
        }
    }
    // Gérer l'apparition de vagues d'ennemis 
    private IEnumerator AjouterVagueEnnemisPeriodiquement()
    {
        while (true)
        {
            yield return new WaitForSeconds(tempsEntreVagues);
            AjouterVagueEnnemi();
        }
    }
    public void AjouterVagueEnnemi()
    {
        StrategieApparitionVague strategie = strategiesApparitionVague[Random.Range(0, strategiesApparitionVague.Length)];
        GameObject[] ennemis = strategie.ApparaitreVague(prefabEnnemis, nbEnnemisParVague, joueur.transform.position);
        foreach (var ennemi in ennemis)
        {
            if (ennemi == null) { Debug.Log("ennemi null"); continue; }
            InitialiserEnnemi(ennemi, true, false);
        }
    }

    private void RafraichirCompteurCreaturesLibres()
    {
        txtCompteurCreaturesLibres.text = nbCreaturesLibres + "";
    }

    private void RafraichirCompteurCreaturesContenues()
    {
        txtCompteurCreaturesContenues.text = nbCreaturesContenues + "";
    }

    public void ApparaitreEnnemi(bool faireEnnemiSaut)
    {
        int randomEnnemiIndex = Random.Range(0, prefabEnnemis.Length);
        var prefab = prefabEnnemis[randomEnnemiIndex];


        // Rien � cet endroit : on peut Instantiate() le prefab
        GameObject ennemi = UtilitaireApparitionPrefab.TentativeApparaitre(prefab);
        if (ennemi == null) return;

        InitialiserEnnemi(ennemi, false, faireEnnemiSaut);
    }
    // Initialiser l'ennemi
    private void InitialiserEnnemi(GameObject ennemi, bool vienDeVague, bool faireEnnemiSaut)
    {
        ComportementEnnemi scriptEnnemi = ennemi.GetComponent<ComportementEnnemi>();
        scriptEnnemi.Initialiser(joueur, vienDeVague, faireEnnemiSaut);

        scriptEnnemi.OnMort += EnnemiMort;

        nbCreaturesLibres++;
        RafraichirCompteurCreaturesLibres();
        RafraichirCompteurCreaturesContenues();
        tousEnnemis.Add(ennemi.GetComponent<ComportementEnnemi>());
    }

    void EnnemiMort(ComportementEnnemi ennemi)
    {
        tousEnnemis.Remove(ennemi);
        nbCreaturesLibres--;
        RafraichirCompteurCreaturesLibres();

        nbCreaturesContenues++;
        RafraichirCompteurCreaturesContenues();
    }
    // Faire tous les ennemis vivants s'enfuire
    private void TerroriserMonstres()
    {
        foreach (ComportementEnnemi ennemi in tousEnnemis)
        {
            ennemi.ChangerEtat(ennemi.etatFuite);
        }
    }
}