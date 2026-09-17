using UnityEngine;
// Stratégie d'apparition de vague au hasard
public class StrategieApparitionHasard : StrategieApparitionVague
{
    public override GameObject[] ApparaitreVague(GameObject[] prefabs, int nbrEnnemis, Vector3 oliviaPosition)
    {
        GameObject[] ennemis = new GameObject[nbrEnnemis];
        for (int i = 0; i < nbrEnnemis; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            ennemis[i] = UtilitaireApparitionPrefab.TentativeApparaitre(prefab);
        }
        return ennemis;
    }
}
