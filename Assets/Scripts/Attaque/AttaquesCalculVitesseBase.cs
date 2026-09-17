using UnityEngine;
// Classe abstraite pour les attaques qui calculent leur vitesse d'attaque comme l'attaque de base
public abstract class AttaquesCalculVitesseBase : Attaque
{
    protected float tempsEntreAttaque = 1;

    public AttaquesCalculVitesseBase(ScriptAttaque scriptAttaque) : base(scriptAttaque)
    {
        tempsEntreAttaque = 10.0f / Mathf.Sqrt(100.0f * (scriptAttaque.niveauAttaque));
    }

    public override void OnNiveauAttaqueAmeliore()
    {
        if (!EstDebloquee) return;
        tempsEntreAttaque = 10.0f / Mathf.Sqrt(100.0f * (scriptAttaque.niveauAttaque)); ;
    }
}