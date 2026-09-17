using System;
using UnityEngine;
// Script pour la leure d'Olivia
public class LeureScript : MonoBehaviour, ICible
{
    public event Action OnDetruit;
    public Vector3 position => transform.position;
    public void PerdreVie(int dommage)
    {
        OnDetruit?.Invoke();
        Destroy(gameObject);
    }
}
