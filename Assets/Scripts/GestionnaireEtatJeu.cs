using System.IO;
using UnityEngine;
// Gérer la sauvegarde et le chargement de l'état du jeu
public class GestionnaireEtatJeu : MonoBehaviour
{
    [SerializeField] private GestionnaireJeu gestionnaireJeu;
    private float argent = 0;
    private string fichierSauvegarde;
    void Awake()
    {
        fichierSauvegarde = Application.persistentDataPath + "/etat-jeu.json"; // Définir le chemin du fichier de sauvegarde
        ChargerEtatJeu();
    }
    void Start()
    {
        gestionnaireJeu.OnJeuTermine += GererArgentFinJeu;
    }
    // Gerer l'argent du joueur quand la partie se termine
    private void GererArgentFinJeu(int creaturesContenues)
    {
        argent = gestionnaireJeu.Joueur.Argent;
        switch (ParametresJeu.Instance.modeCourant)
        {
            case ParametresJeu.Modes.Facile:
                argent += creaturesContenues;
                break;
            case ParametresJeu.Modes.Difficile:
                argent += creaturesContenues * 2;
                break;
            default:
                break;
        }
        SauvegarderEtatJeu();
    }
    // Sauvegarder l'état du jeu avec la sérialisation JSON
    private void SauvegarderEtatJeu()
    {
        EtatJeu etatJeu = new EtatJeu()
        {
            argentAmasse = argent,
            bouleFeuEstDebloque = gestionnaireJeu.Joueur.BouleFeuEstDebloque(),
            livreEstDebloque = gestionnaireJeu.Joueur.LivreEstDebloque()
        };
        string json = JsonUtility.ToJson(etatJeu);
        File.WriteAllText(fichierSauvegarde, json);
    }
    // Charger l'état du jeu à partir du fichier de sauvegarde
    private void ChargerEtatJeu()
    {
        if (!File.Exists(fichierSauvegarde))
        {
            Debug.Log("Fichier non existant");
            return;
        }
        string json = File.ReadAllText(fichierSauvegarde);
        EtatJeu etatJeu = JsonUtility.FromJson<EtatJeu>(json);
        argent = etatJeu.argentAmasse;
        gestionnaireJeu.Joueur.ChangerArgent(argent);
        gestionnaireJeu.Joueur.ChangerUtilisabiliteBouleFeu(etatJeu.bouleFeuEstDebloque);
        gestionnaireJeu.Joueur.ChangerUtilisabiliteLivre(etatJeu.livreEstDebloque);
    }
}
