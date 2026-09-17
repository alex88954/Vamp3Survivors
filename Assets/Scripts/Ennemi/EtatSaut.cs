using System.Collections.Generic;
using UnityEngine;
// Gerer l'état de saut de l'ennemi
public class EtatSaut : EtatEnnemi
{
    Vector3 targetPos = Vector3.zero;
    Vector3 startPos = Vector3.zero;
    Light lumiereSaut;
    private float duration = 3;
    private float tempsPasse = 0;
    public EtatSaut(ComportementEnnemi comportementEnnemi) : base(comportementEnnemi)
    {
        sujet.conteneurLumiereSaut.SetActive(true);
        lumiereSaut = sujet.conteneurLumiereSaut.GetComponent<Light>();
    }
    public override void Entrer()
    {
        sujet.agent.enabled = false;

        startPos = sujet.transform.position;
        targetPos = sujet.scriptJoueur.position;

        sujet.conteneurLumiereSaut.transform.position = new Vector3(targetPos.x, 30, targetPos.z);

    }
    public override void Executer(float deltaTime)
    {
        tempsPasse += deltaTime;

        float p = tempsPasse / duration;
        float h = 3;

        // Mouvement Horizontal
        Vector3 position = Vector3.Lerp(startPos, targetPos, p);


        // Mouvement Vertical
        position.y = 4 * h * p * (1 - p);

        // Faire le mouvement
        sujet.transform.position = position;

        // Augmenter lumière
        GrandirLumiere(p);

        if (Vector3.Distance(targetPos, sujet.transform.position) < 1)
        {
            FaireDegats();
            sujet.ChangerEtat(sujet.etatPoursuite);
        }
    }
    public override void Sortir()
    {
        sujet.agent.enabled = true;
        sujet.conteneurLumiereSaut.SetActive(false);
    }
    // Gerer les dommages à l'atterrissage
    private void FaireDegats()
    {
        RaycastHit[] proximite = Physics.SphereCastAll(sujet.transform.position, 5, Vector3.down, 0.1f); // Trouver les cibles à proximité
        foreach (var e in proximite)
        {
            if (e.collider.TryGetComponent(out ComportementEnnemi ennemi))
            {
                if (ennemi == sujet) continue;
                ennemi.PerdreVie(2);
            }
            else if (e.collider.TryGetComponent(out ICible cible))
            {
                cible.PerdreVie(2);
            }
        }
    }
    // Agrandir la lumière progressivement
    private void GrandirLumiere(float p)
    {
        lumiereSaut.spotAngle = p * sujet.maxLightSpotAngle;
        lumiereSaut.innerSpotAngle = p * sujet.maxLightSpotAngle;
    }
}