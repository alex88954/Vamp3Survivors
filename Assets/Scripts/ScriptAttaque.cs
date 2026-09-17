using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;
// Script qui gère le rayon d'attaque et le déblocage des attaques de Olivia
public class ScriptAttaque : MonoBehaviour
{
    public HashSet<ComportementEnnemi> ennemiSet = new();

    public float rayon = 2f;
    public int dommages = 5;
    public int niveauAttaque = 1;

    private SphereCollider sphereCollider;
    private VisualEffect vfxManager;
    private AttaqueBase attaqueBase;
    private AttaqueBouleFeu attaqueBouleFeu;
    [SerializeField] private GameObject bouleFeuPrefab;
    private AttaqueLivre attaqueLivre;
    [SerializeField] GameObject livre;
    private List<Attaque> attaques = new List<Attaque>();
    [SerializeField] private Transform _oliviaTransform;


    void Start()
    {
        sphereCollider = GetComponent<SphereCollider>();
        vfxManager = GetComponent<VisualEffect>();

        //RecalculTempsAttaque();
        attaqueBase = new AttaqueBase(this);
        attaques.Add(attaqueBase);
        StartCoroutine(attaqueBase.Attaquer());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ComportementEnnemi ennemi))
        {
            ennemiSet.Add(ennemi);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out ComportementEnnemi ennemi))
        {
            ennemiSet.Remove(ennemi);
        }
    }
    // Méthodes d'amélioration des attaques
    public void AmeliorerRayon(int rayonGagne)
    {
        rayon += rayonGagne;
        foreach (Attaque attaque in attaques)
        {
            attaque.OnRayonAmeliore();
        }
    }

    public void AmeliorerRapidite(int vitesseAttaqueGagne)
    {
        niveauAttaque += vitesseAttaqueGagne;
        foreach (Attaque attaque in attaques)
        {
            attaque.OnNiveauAttaqueAmeliore();
        }
    }
    // Méthodes pour débloquer des attaques
    public void DebloquerBouleFeu()
    {
        if (attaqueBouleFeu != null) return;
        attaqueBouleFeu = new AttaqueBouleFeu(this)
        {
            EstDebloquee = true
        };
        attaques.Add(attaqueBouleFeu);
        StartCoroutine(attaqueBouleFeu.Attaquer());

    }
    public void DebloquerLivre()
    {
        if (attaqueLivre != null) return;
        attaqueLivre = new AttaqueLivre(this)
        {
            EstDebloquee = true
        };
        attaqueLivre.Initialiser(livre);
        attaques.Add(attaqueLivre);
    }
    // Getters et Setters
    public VisualEffect GetVFXManager()
    {
        return vfxManager;
    }
    public SphereCollider GetSphereCollider()
    {
        return sphereCollider;
    }
    public VisualEffect SetVFXManager(VisualEffect vfx)
    {
        vfxManager = vfx;
        return vfxManager;
    }
    public SphereCollider SetSphereCollider(SphereCollider collider)
    {
        sphereCollider = collider;
        return sphereCollider;
    }
    public Vector3 GetOliviaPosition()
    {
        return _oliviaTransform.position;
    }
    public GameObject GetBouleFeuPrefab()
    {
        return bouleFeuPrefab;
    }
}