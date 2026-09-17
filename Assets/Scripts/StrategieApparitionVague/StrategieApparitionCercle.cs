using UnityEngine;
// Stratégie d'apparition de vague en cercle autour d'Olivia
public class StrategieApparitionCercle : StrategieApparitionVague
{
    public override GameObject[] ApparaitreVague(GameObject[] prefabs, int nbrEnnemis, Vector3 oliviaPosition)
    {
        float angleEntreEnnemis = 360 / nbrEnnemis;
        float rayon = 3.5f;

        GameObject[] ennemis = new GameObject[nbrEnnemis];

        for (int i = 0; i < nbrEnnemis; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            float radius = prefab.GetComponent<CapsuleCollider>().radius;

            float angle = i * angleEntreEnnemis;

            float angleRad = angle * Mathf.Deg2Rad; // Convertir l'angle en radians

            float x = Mathf.Cos(angleRad) * rayon;
            float z = Mathf.Sin(angleRad) * rayon;

            Vector3 position = oliviaPosition + new Vector3(x, radius, z);

            ennemis[i] = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefab, position);
        }
        return ennemis;
    }
}
