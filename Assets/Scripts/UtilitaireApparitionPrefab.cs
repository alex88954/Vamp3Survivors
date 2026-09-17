using UnityEngine;
// Classe utilitaire pour faire apparaître un prefab
public static class UtilitaireApparitionPrefab
{
    public static GameObject ApparaitreSiLibre(GameObject prefab, Vector3 position)
    {
        float radius = prefab.GetComponent<CapsuleCollider>().radius;
        // On prend 0.9*le radius pour �viter de d�tecter le plancher dans la collision
        if (Physics.CheckSphere(position, radius * 0.9f))
        {
            // D�j� un objet � l'endroit pr�vu
            return null;
        }

        // Rien � cet endroit : on peut Instantiate() le prefab
        GameObject ennemi = Object.Instantiate(prefab, position, Quaternion.identity);
        return ennemi;
    }
    public static GameObject ApparaitreAuHasard(GameObject prefab)
    {
        float radius = prefab.GetComponent<CapsuleCollider>().radius;

        Vector3 position = new Vector3(0f, radius, 0f);
        position.x = Random.Range(-50f, 50f);
        position.z = Random.Range(-50f, 50f);
        return ApparaitreSiLibre(prefab, position);
    }
    public static GameObject TentativeApparaitre(GameObject prefab)
    {
        GameObject ennemi = null;
        int attempts = 0;
        while (attempts < 5)
        {
            ennemi = ApparaitreAuHasard(prefab);
            if (ennemi != null)
            {
                break;
            }
            attempts++;
        }
        return ennemi;
    }
}