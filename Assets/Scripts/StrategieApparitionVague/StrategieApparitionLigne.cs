using UnityEngine;
// Stratégie d'apparition de vague en ligne avec un cercle de base autour d'Olivia
public class StrategieApparitionLigne : StrategieApparitionVague
{
    public override GameObject[] ApparaitreVague(GameObject[] prefabs, int nbrEnnemis, Vector3 oliviaPosition)
    {
        GameObject[] ennemis = new GameObject[nbrEnnemis];

        float angle = Random.Range(0, 360);
        float angleRad = angle * Mathf.Deg2Rad;

        int rayon = 5;

        bool horizontal = Random.value > 0.5f;

        int direction = Random.value > 0.5f ? 1 : -1;

        for (int i = 0; i < nbrEnnemis; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            float radius = prefab.GetComponent<CapsuleCollider>().radius;

            float offsetCercleX = Mathf.Cos(angleRad) * rayon;
            float offsetCercleZ = Mathf.Sin(angleRad) * rayon;

            float offsetLigneX = horizontal ? i * direction : 0;
            float offsetLigneZ = horizontal ? 0 : i * direction;

            Vector3 position = new Vector3(oliviaPosition.x + offsetCercleX + offsetLigneX, radius, oliviaPosition.z + offsetCercleZ + offsetLigneZ);

            ennemis[i] = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefab, position);
        }
        return ennemis;
    }
}
