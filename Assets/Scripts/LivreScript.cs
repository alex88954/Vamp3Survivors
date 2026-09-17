using UnityEngine;
// Script pour le comportement du livre
public class LivreScript : MonoBehaviour
{
    private int _dommages = 0;
    private Transform pivotTransform;
    [SerializeField] private int vitesseRotation = 5;
    private float vitesseOrbite = 0;
    void Start()
    {
        pivotTransform = transform.parent;
    }

    void Update()
    {
        transform.Rotate(Vector3.up * vitesseRotation * Time.deltaTime); // Faire tourner le livre sur lui-même
        pivotTransform.Rotate(Vector3.up * vitesseOrbite * Time.deltaTime); // Faire orbiter le livre autour du pivot

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ComportementEnnemi ennemi))
        {
            ennemi.PerdreVie(_dommages);
        }
    }
    // Initialiser les dommages du livre
    public void SetDommage(int dommages)
    {
        _dommages = dommages;
    }
    // Changer la distance du livre par rapport au pivot
    public void ChangerDistance(float distance)
    {
        transform.localPosition = new Vector3(distance, 0.75f, 0);
    }
    // Changer la vitesse d'orbite du livre
    public void ChangerVitesse(float vitesse)
    {
        vitesseOrbite = vitesse;
    }
}
