using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ComportementEnnemi : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Animator animateur;
    [HideInInspector] public ComportementPersonnage scriptJoueur;

    public float distancePoursuiteToAttaque = 5f;
    public float distanceAttaqueToPoursuite = 2f;
    public float tempsEntreAttaque = 2f;
    public int dommages = 2;
    public int experiences = 5;
    public int vie = 10;

    private EtatEnnemi etatCourant;
    public EtatPoursuite etatPoursuite;
    public EtatFuite etatFuite;
    public EtatAttaque etatAttaque;

    [SerializeField] private GameObject experiencePrefab;
    [SerializeField] private GameObject bouleViePrefab;
    [SerializeField] private GameObject boulePeurPrefab;

    private AnimationViePerdue animationViePerdue;

    public event Action<ComportementEnnemi> OnMort;

    [SerializeField] public bool EstPeureux;

    [SerializeField] public GameObject conteneurLumiereSaut;
    [SerializeField] public float maxLightSpotAngle = 18;

    public ICible cibleCourante;
    // Initialiser le comportement de l'ennemi
    public void Initialiser(ComportementPersonnage _scriptJoueur, bool vienDeVague, bool faireEnnemiSaut)
    {
        scriptJoueur = _scriptJoueur;
        CiblerJoueur();
        _scriptJoueur.OnLeureApparition += CiblerLeure;
        _scriptJoueur.OnLeureDetruit += CiblerJoueur;
        if (!vienDeVague)
        {
            float rng = UnityEngine.Random.value;
            if (rng < 0.25f || faireEnnemiSaut)
            {
                ChangerEtat(new EtatSaut(this));
            }
        }
    }

    // Cibler leure si la distance est inférieure à 5 lors de son apparition
    public void CiblerLeure(ICible leure)
    {
        float distanceALeure = Vector3.Distance(transform.position, leure.position);
        if (distanceALeure < 5)
        {
            cibleCourante = leure;
        }
    }
    public void CiblerJoueur()
    {
        cibleCourante = scriptJoueur;
    }
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animateur = GetComponent<Animator>();
        animationViePerdue = GetComponent<AnimationViePerdue>();
    }

    void Start()
    {


        etatPoursuite = new EtatPoursuite(this);
        etatFuite = new EtatFuite(this);
        etatAttaque = new EtatAttaque(this);

        if (etatCourant == null)
        {
            etatCourant = etatPoursuite;
            etatCourant.Entrer();
        }


    }

    void Update()
    {
        etatCourant.Executer(Time.deltaTime);
    }

    public void ChangerEtat(EtatEnnemi nouvelEtat)
    {
        if (etatCourant == nouvelEtat) return;
        if (etatCourant != null)
        {
            etatCourant.Sortir();
        }
        etatCourant = nouvelEtat;
        etatCourant.Entrer();
    }

    public void PerdreVie(int dommage)
    {
        vie -= dommage;
        if (vie <= 0) Mourir();
        animationViePerdue.Demarrer();
    }

    private void Mourir()
    {
        Vector3 positionOrb = new Vector3(transform.position.x, 0.5f, transform.position.z);
        GameObject drop = Instantiate(ChoisirDrop(), positionOrb, Quaternion.identity);
        if (drop == experiencePrefab)
        {
            drop.GetComponent<ObjetExperience>().experienceGagne = experiences;
        }
        Destroy(gameObject);
        scriptJoueur.OnLeureApparition -= CiblerLeure;
        scriptJoueur.OnLeureDetruit -= CiblerJoueur;
        OnMort?.Invoke(this);
    }

    public bool APeur()
    {
        return EstPeureux && vie == scriptJoueur.Dommages;
    }
    // Choisir un drop aléatoire
    public GameObject ChoisirDrop()
    {
        int rng = UnityEngine.Random.Range(1, 101);
        switch (rng)
        {
            case <= 5:
                return boulePeurPrefab;
            case <= 15:
                return bouleViePrefab;
            case <= 100:
                return experiencePrefab;
        }
        return null;
    }

}