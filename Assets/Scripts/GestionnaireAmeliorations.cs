using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
public class GestionnaireAmeliorations : MonoBehaviour
{
    [SerializeField]
    private ComportementPersonnage scriptJoueur;

    [SerializeField]
    private ComportementCamera comportementCamera;

    [SerializeField]
    private GameObject panel;

    [SerializeField] private GameObject conteneurBoutonAttaqueBouleFeu;
    [SerializeField] private GameObject conteneurBoutonAttaqueLivre;

    [SerializeField] private TMP_Text textArgent;

    [SerializeField] private TMP_Text textAmelioration1;
    [SerializeField] private TMP_Text textAmelioration2;
    [SerializeField] private TMP_Text textAmelioration3;

    string amelioration1;
    string amelioration2;
    string amelioration3;
    private List<TMP_Text> textsAmelioration = new List<TMP_Text>();

    [SerializeField] private int vieGagne = 4;
    [SerializeField] private int rayonGagne = 1;
    [SerializeField] private int vitesseAttaqueGagne = 1;
    [SerializeField] private float vitesseCourseGagne = 1.25f;
    [SerializeField] private int dommagesGagne = 1;
    private Dictionary<string, float> ameliorations;

    void Start()
    {
        ameliorations = new Dictionary<string, float>
        {
                {"Vie", vieGagne},
                {"Rayon", rayonGagne},
                {"VitesseAttaque", vitesseAttaqueGagne},
                {"VitesseCourse", vitesseCourseGagne},
                {"Dommages", dommagesGagne}
        };
        textsAmelioration.Add(textAmelioration1);
        textsAmelioration.Add(textAmelioration2);
        textsAmelioration.Add(textAmelioration3);

        scriptJoueur.OnAmeliorationAtteinte += OuvrirMenu;
        OuvrirMenu();
    }

    // Contrôle de menu
    private void OuvrirMenu()
    {
        Time.timeScale = 0;
        comportementCamera.ArreterBrassage();
        panel.SetActive(true);
        scriptJoueur.OnArgentChange += ChangerAffichageArgent;
        GererAffichageAmeliorations();
        conteneurBoutonAttaqueBouleFeu.SetActive(!scriptJoueur.BouleFeuEstDebloque());
        conteneurBoutonAttaqueLivre.SetActive(!scriptJoueur.LivreEstDebloque());
        ChangerAffichageArgent();

    }

    private void FermerMenu()
    {
        Time.timeScale = 1;
        panel.SetActive(false);
    }
    // Gerer la quantité d'argent sur le menu
    private void ChangerAffichageArgent()
    {
        textArgent.text = scriptJoueur.Argent.ToString("0.##");
    }
    // Méthodes d'amélioration
    public void AmeliorerVie()
    {
        scriptJoueur.AmeliorerVie(vieGagne);
        FermerMenu();
    }

    public void AmeliorerRayon()
    {
        scriptJoueur.AmeliorerRayon(rayonGagne);
        FermerMenu();
    }

    public void AmeliorerVitesseAttaque()
    {
        scriptJoueur.AmeliorerVitesseAttaque(vitesseAttaqueGagne);
        FermerMenu();
    }
    public void AmeliorerVitesseCourse()
    {
        scriptJoueur.AmeliorerVitesseCourse(vitesseCourseGagne);
        FermerMenu();
    }
    public void AmeliorerDommage()
    {
        scriptJoueur.AmeliorerVitesseAttaque(dommagesGagne);
        FermerMenu();
    }
    // Méthodes pour les boutons du menu
    public void OnAmelioration1Clicked()
    {
        Ameliorer(amelioration1);
    }
    public void OnAmelioration2Clicked()
    {
        Ameliorer(amelioration2);
    }
    public void OnAmelioration3Clicked()
    {
        Ameliorer(amelioration3);
    }
    // Faire l'amélioration selon celle reçue en paramètre
    private void Ameliorer(string n)
    {
        switch (n)
        {
            case "Vie":
                AmeliorerVie();
                break;
            case "Rayon":
                AmeliorerRayon();
                break;
            case "VitesseAttaque":
                AmeliorerVitesseAttaque();
                break;
            case "VitesseCourse":
                AmeliorerVitesseCourse();
                break;
            case "Dommages":
                AmeliorerDommage();
                break;
            default:
                break;
        }
    }
    // Initialiser le texte des boutons du menu avec des améliorations choisies au hasard
    private void GererAffichageAmeliorations()
    {
        List<int> rngs = new List<int>();
        while (rngs.Count < 3)
        {
            int rng = Random.Range(1, 6);
            if (!rngs.Contains(rng))
            {
                rngs.Add(rng);
            }
        }
        rngs = rngs.OrderBy(e => Random.value).ToList(); // Mélanger
        int compteurRng = 1;
        int CompteurBouton = 0;
        foreach (KeyValuePair<string, float> pair in ameliorations)
        {
            if (rngs.Contains(compteurRng))
            {
                switch (pair.Key)
                {
                    case "Vie":
                        textsAmelioration[CompteurBouton].text = $"Vie + {pair.Value}";
                        SetBoutonAmelioration(CompteurBouton, pair.Key);
                        CompteurBouton++;
                        break;
                    case "Rayon":
                        textsAmelioration[CompteurBouton].text = $"Rayon + {pair.Value}";
                        SetBoutonAmelioration(CompteurBouton, pair.Key);
                        CompteurBouton++;
                        break;
                    case "VitesseAttaque":
                        textsAmelioration[CompteurBouton].text = $"Attaque + {pair.Value}";
                        SetBoutonAmelioration(CompteurBouton, pair.Key);
                        CompteurBouton++;
                        break;
                    case "VitesseCourse":
                        textsAmelioration[CompteurBouton].text = $"Vitesse de course + {pair.Value}";
                        SetBoutonAmelioration(CompteurBouton, pair.Key);
                        CompteurBouton++;
                        break;
                    case "Dommages":
                        textsAmelioration[CompteurBouton].text = $"Dommages + {pair.Value}";
                        SetBoutonAmelioration(CompteurBouton, pair.Key);
                        CompteurBouton++;
                        break;
                    default:
                        break;
                }
            }
            compteurRng++;
        }
    }
    // Initialiser le bouton pour le clique
    private void SetBoutonAmelioration(int ameliorationNum, string ameliorationCible)
    {
        switch (ameliorationNum)
        {
            case 0:
                amelioration1 = ameliorationCible;
                break;
            case 1:
                amelioration2 = ameliorationCible;
                break;
            case 2:
                amelioration3 = ameliorationCible;
                break;
            default:
                break;
        }
    }
    // Méthodes pour les boutons d'attaque
    public void OnBouleFeuClicked()
    {
        if (scriptJoueur.Argent >= 150)
        {
            scriptJoueur.GagnerArgent(-150);
            DebloquerBouleFeu();
        }
    }
    public void OnLivreClicked()
    {
        if (scriptJoueur.Argent >= 100)
        {
            scriptJoueur.GagnerArgent(-100);
            DebloquerLivre();
        }
    }
    // Débloquer les attaques et cacher les boutons
    public void DebloquerBouleFeu()
    {
        scriptJoueur.ChangerUtilisabiliteBouleFeu(true);
        conteneurBoutonAttaqueBouleFeu.SetActive(!scriptJoueur.BouleFeuEstDebloque());
        Debug.Log("Boule feu debloquée");
    }
    public void DebloquerLivre()
    {
        scriptJoueur.ChangerUtilisabiliteLivre(true);
        conteneurBoutonAttaqueLivre.SetActive(!scriptJoueur.LivreEstDebloque());
        Debug.Log("Livre debloquée");
    }
}
