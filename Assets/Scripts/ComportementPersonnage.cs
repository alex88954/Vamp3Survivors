using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ComportementPersonnage : MonoBehaviour, ICible
{
    [SerializeField] private float vitesseBase = 10f;
    [SerializeField] private float vitesseRotation = 180f;

    private int vies;
    private int maxVies;
    private int experience = 0;
    private float _argent = 0;
    private float multiplicateurSprint = 2f;


    private int experienceRequise;
    [SerializeField] private TMP_Text affichagePointsXp;
    [SerializeField] private BarreVie barreVie;
    [SerializeField] private VisualEffectAsset[] vfx;
    [SerializeField] private GameObject oliviaLeure;
    [SerializeField] private Image logoLeure;
    private bool leureEnCooldown = false;
    public Vector3 position => transform.position;
    public float Argent => _argent;

    public event Action OnMort;
    public event Action OnAmeliorationAtteinte;
    public event Action OnLeureDetruit;
    public event Action<ICible> OnLeureApparition;
    public event Action OnArgentChange;
    public Action OnboulePeurCollected;
    [SerializeField] private ScriptAttaque scriptAttaque;
    public int Dommages => scriptAttaque.dommages;

    private InputAction mouvementAction;
    private InputAction sprintAction;
    private InputAction leureAction;
    private Animator animator;
    private CharacterController characterController;

    private Quaternion targetRotation;

    private AnimationViePerdue animationViePerdue;
    [SerializeField] private ComportementCamera comportementCamera;

    private bool estMort = false;

    private bool bouleFeuDebloque = false;
    private bool livreDebloque = false;

    void Start()
    {
        mouvementAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        leureAction = InputSystem.actions.FindAction("Jump");
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        vies = ParametresJeu.Instance.VieDepart;
        maxVies = ParametresJeu.Instance.VieDepart;

        experienceRequise = ParametresJeu.Instance.ExperienceRequise;

        animationViePerdue = GetComponent<AnimationViePerdue>();

    }

    void Update()
    {
        bool isRunning = sprintAction.IsPressed();
        Vector2 mouvement = mouvementAction.ReadValue<Vector2>();

        float vitesseApplique = vitesseBase;
        if (isRunning) vitesseApplique *= multiplicateurSprint;

        Vector3 mouvementApplique = new Vector3(mouvement.x, 0f, mouvement.y) * vitesseApplique;
        characterController.SimpleMove(mouvementApplique);

        if (mouvementApplique.magnitude > 0)
        {
            targetRotation = Quaternion.LookRotation(mouvementApplique.normalized);
        }

        transform.rotation =
            Quaternion.RotateTowards(transform.rotation, targetRotation, vitesseRotation * Time.deltaTime);

        animator.SetFloat("Vitesse", mouvementApplique.magnitude);

        RefreshBarreVie();

        // Apparition de leure
        if (leureAction.WasPressedThisFrame() && !leureEnCooldown)
        {
            GameObject leure = Instantiate(oliviaLeure, transform.position, transform.rotation);
            LeureScript leureScript = leure.GetComponent<LeureScript>();
            OnLeureApparition?.Invoke(leureScript);
            leureScript.OnDetruit += GererLeureDetruite;
            leureEnCooldown = true;
            StartCoroutine(CooldownLeure());
        }
        GererLogoLeure();
    }
    private void GererLeureDetruite()
    {
        OnLeureDetruit?.Invoke();
    }

    // Gérer le temps de recharge de leure
    private IEnumerator CooldownLeure()
    {
        yield return new WaitForSeconds(7f);
        leureEnCooldown = false;
    }
    // Gérer l'affichage du logo de leure
    private void GererLogoLeure()
    {
        if (estMort) return;
        if (leureEnCooldown)
        {
            logoLeure.enabled = false;
        }
        else
        {
            logoLeure.enabled = true;
        }
    }

    private void RefreshBarreVie()
    {
        barreVie.SetPourcentage(vies * 100f / maxVies);
    }

    public void GagnerVies(int nb)
    {
        vies += nb;
        if (vies > maxVies)
            vies = maxVies;

        RefreshBarreVie();
    }

    public void PerdreVie(int dommage)
    {
        vies -= dommage;
        RefreshBarreVie();

        if (vies <= 0)
        {
            OnMort?.Invoke();
            estMort = true;
            logoLeure.enabled = false;
        }

        animationViePerdue.Demarrer();
        comportementCamera.Brasser();
    }

    public void GagnerExperience(int experienceGagne)
    {
        ChangerExperience(experience + experienceGagne);

        if (experience >= experienceRequise)
        {
            OnAmeliorationAtteinte.Invoke();
        }
    }

    public void AmeliorerVie(int vieGagne)
    {
        maxVies += vieGagne;
        vies = maxVies;
        RefreshBarreVie();

        ChangerExperience(0);
    }

    public void AmeliorerRayon(int rayonGagne)
    {
        scriptAttaque.AmeliorerRayon(rayonGagne);
        ChangerExperience(0);
    }

    public void AmeliorerVitesseAttaque(int vitesseAttaqueGagne)
    {
        scriptAttaque.AmeliorerRapidite(vitesseAttaqueGagne);
        ChangerExperience(0);
    }
    public void AmeliorerVitesseCourse(float vitesseCourse)
    {
        multiplicateurSprint *= vitesseCourse;
    }

    private void ChangerExperience(int nombreExperience)
    {
        experience = nombreExperience;
        affichagePointsXp.text = experience.ToString();
    }
    // Changer la quantité d'argent
    public void ChangerArgent(float argent)
    {
        _argent = argent;
        OnArgentChange?.Invoke();
    }
    public void GagnerArgent(float argentGagne)
    {
        _argent += argentGagne;
        OnArgentChange?.Invoke();
    }
    public void RedonnerMaxVies()
    {
        vies = maxVies;
        RefreshBarreVie();
    }
    public void ChangerUtilisabiliteBouleFeu(bool estDebloquee)
    {
        bouleFeuDebloque = estDebloquee;
        if (bouleFeuDebloque)
        {
            scriptAttaque.DebloquerBouleFeu();
        }
    }
    public void ChangerUtilisabiliteLivre(bool estDebloquee)
    {
        livreDebloque = estDebloquee;
        if (livreDebloque)
        {
            scriptAttaque.DebloquerLivre();
        }
    }
    // Getters pour vérifier si les attaques sont débloquées
    public bool BouleFeuEstDebloque()
    {
        return bouleFeuDebloque;
    }
    public bool LivreEstDebloque()
    {
        return livreDebloque;
    }
}