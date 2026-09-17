using System.Collections;
using UnityEngine;
// Script pour gerer les améliorations de l'attaque du livre
public class AttaqueLivre : Attaque
{
    private GameObject _livre;
    private LivreScript _livreScript;
    private float vitesseOrbiteLivre = 1;
    private float vitesseOrbiteInitiale = 150;
    public override bool EstDebloquee { get; set; } = false;

    public AttaqueLivre(ScriptAttaque scriptAttaque) : base(scriptAttaque)
    {

    }
    public override void OnNiveauAttaqueAmeliore()
    {
        if (!EstDebloquee) return;
        vitesseOrbiteLivre = vitesseOrbiteInitiale * (scriptAttaque.niveauAttaque / 2f);
        _livreScript.ChangerVitesse(vitesseOrbiteLivre);
    }
    public override void OnRayonAmeliore()
    {
        if (!EstDebloquee) return;
        _livre.transform.localScale += Vector3.one * 0.25f;
        _livreScript.ChangerDistance(scriptAttaque.rayon);
    }
    public override IEnumerator Attaquer()
    {
        yield break;
    }
    public void Initialiser(GameObject livre)
    {
        if (!EstDebloquee) return;
        _livre = livre;
        _livreScript = livre.GetComponent<LivreScript>();
        OnNiveauAttaqueAmeliore();
        _livreScript.ChangerDistance(scriptAttaque.rayon);
        _livre.SetActive(true);
    }
}
