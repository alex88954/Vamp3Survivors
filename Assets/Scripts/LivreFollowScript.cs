using UnityEngine;
// Script pour faire suivre le livre à Olivia
public class LivreFollowScript : MonoBehaviour
{
    [SerializeField] private Transform _oliviaTransform;
    void LateUpdate()
    {
        transform.position = _oliviaTransform.position;
    }
}
