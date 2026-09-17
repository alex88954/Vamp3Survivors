using UnityEngine;
// Script pour le comportement de la boule de feu
public class BouleFeuScript : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 direction;
    private int _dommages;
    private float _scale;
    [SerializeField] private float vitesse = 5f;
    SphereCollider sphereCollider;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        transform.localScale = Vector3.one * _scale;
        sphereCollider.radius = _scale;
        direction = Random.insideUnitSphere;
        direction.y = 0;
        direction = direction.normalized;


        rb.linearVelocity = direction * vitesse;

        Destroy(gameObject, 5);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ComportementEnnemi ennemi))
        {
            ennemi.PerdreVie(_dommages);
        }
    }
    // Initialiser les variables de la boule de feu
    public void Initialiser(int dommages, float scale)
    {
        _dommages = dommages;
        _scale = scale;
    }
}
