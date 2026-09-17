using UnityEngine;
// Interface pour les cibles 
public interface ICible
{
    Vector3 position { get; }
    void PerdreVie(int dommage);
}