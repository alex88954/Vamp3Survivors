using UnityEngine;
// Classe abstraite pour la stratégie d'apparition des vagues d'ennemis
public abstract class StrategieApparitionVague
{
    public abstract GameObject[] ApparaitreVague(GameObject[] prefabs, int nbrEnnemis, Vector3 oliviaPosition);
}
