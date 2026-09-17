using System.Collections;
using UnityEngine;
// Script pour gerer les améliorations de l'attaque de boule de feu et l'instanciation
public class AttaqueBouleFeu : AttaquesCalculVitesseBase
{
    private float scaleBouleFeu = 0.5f;
    public override bool EstDebloquee { get; set; } = false;
    public AttaqueBouleFeu(ScriptAttaque scriptAttaque) : base(scriptAttaque)
    {
    }
    public override void OnRayonAmeliore()
    {
        if (!EstDebloquee) return;
        scaleBouleFeu += 0.2f;
    }
    public override IEnumerator Attaquer()
    {
        if (!EstDebloquee) yield break;
        while (true)
        {
            GameObject bouleFeu = Object.Instantiate(scriptAttaque.GetBouleFeuPrefab(), scriptAttaque.GetOliviaPosition(), Quaternion.identity);
            BouleFeuScript bouleFeuScript = bouleFeu.GetComponent<BouleFeuScript>();
            bouleFeuScript.Initialiser(scriptAttaque.dommages, scaleBouleFeu);
            yield return new WaitForSeconds(tempsEntreAttaque);
        }
    }
}