using UnityEngine;
// Script pour qu'un objet ne suive pas son parent
public class ScriptPasSuivreParent : MonoBehaviour
{
    void Start()
    {
        transform.SetParent(null);
    }
}
