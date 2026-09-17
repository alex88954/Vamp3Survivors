using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.VFX;
// Script pour l'attaque de base
public class AttaqueBase : AttaquesCalculVitesseBase
{

    public AttaqueBase(ScriptAttaque scriptAttaque) : base(scriptAttaque)
    {
        OnNiveauAttaqueAmeliore();
    }


    public override void OnRayonAmeliore()
    {
        SphereCollider sphereCollider = scriptAttaque.GetSphereCollider();
        VisualEffect vfxManager = scriptAttaque.GetVFXManager();

        sphereCollider.radius = scriptAttaque.rayon;
        vfxManager.SetFloat("RayonAttaque", scriptAttaque.rayon / 2);

        scriptAttaque.SetSphereCollider(sphereCollider);
        scriptAttaque.SetVFXManager(vfxManager);
    }
    public override IEnumerator Attaquer()
    {
        while (true)
        {
            if (scriptAttaque.ennemiSet.Count == 0)
            {
                yield return new WaitForSeconds(tempsEntreAttaque);
                continue;
            }

            // Choisi un ennemi aleatoire
            int indexAleatoire = Random.Range(0, scriptAttaque.ennemiSet.Count);
            ComportementEnnemi[] ennemiTable = scriptAttaque.ennemiSet.ToArray();
            ComportementEnnemi ennemiRandom = ennemiTable[indexAleatoire];

            // Si existe pas, on le retire et on recommence
            if (ennemiRandom == null)
            {
                scriptAttaque.ennemiSet.Remove(ennemiRandom);
                continue;
            }

            // Sinon on attaque l ennemi
            ennemiRandom.PerdreVie(scriptAttaque.dommages);

            yield return new WaitForSeconds(tempsEntreAttaque);
        }
    }

}
