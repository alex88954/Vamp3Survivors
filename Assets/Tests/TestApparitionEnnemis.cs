using System.Collections;
using UnityEngine;
using NUnit.Framework;
using UnityEditor;
using UnityEngine.TestTools;

public class TestApparitionEnnemis
{
    private GameObject prefabCapsuleSimple;

    [SetUp]
    public void ChargerPrefabs()
    {
        // Vous devez créer ce Prefab dans vos assets
        prefabCapsuleSimple = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Tests/Capsule.prefab");
    }

    [UnityTearDown]
    public IEnumerator DetruireTout()
    {
        // On supprime tous les objets qui ont un Collider
        foreach (Collider collider in GameObject.FindObjectsByType<Collider>(FindObjectsSortMode.None))
        {
            GameObject.Destroy(collider.gameObject);
        }

        // On laisse passer une frame à la fin de chaque test, question que les Destroy() se fassent compléter 
        yield return null;
    }

    [Test]
    public void TestExemple()
    {
        // Arrange
        GameObject objet = GameObject.Instantiate(prefabCapsuleSimple, Vector3.zero, Quaternion.identity);

        // Act
        objet.transform.position += new Vector3(10, 0, 0);

        // Assert
        Assert.AreEqual(10.0f, objet.transform.position.x);
    }
    [Test]
    public void ApparaitreSiLibre_Dans_SceneVide()
    {
        // Arrange
        GameObject objet = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, Vector3.zero);

        // Assert
        Assert.NotNull(objet);
    }
    [Test]
    public void ApparaitreSiLibre_Dans_SceneAvecUneCapsule()
    {
        // Arrange
        GameObject objet1 = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, Vector3.zero);

        // Act
        GameObject objet = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, new Vector3(25, 1, 20));

        // Assert
        Assert.NotNull(objet);
    }
    [Test]
    public void ApparaitreSiLibre_SurAutreCapsule()
    {
        // Arrange
        GameObject objet1 = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, Vector3.zero);

        // Act
        GameObject objet = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, Vector3.zero);

        // Assert
        Assert.IsNull(objet);
    }
    [Test]
    public void ApparaitreSiLibre_TropProcheAutreCapsule()
    {
        // Arrange
        GameObject objet1 = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, Vector3.zero);

        // Act
        GameObject objet = UtilitaireApparitionPrefab.ApparaitreSiLibre(prefabCapsuleSimple, new Vector3(0.5f, 0, 0.5f));

        // Assert
        Assert.IsNull(objet);
    }
    [Test]
    public void ApparaitreAuHasard100Fois()
    {
        // Arrange
        GameObject objet = null;

        // Act
        for (int i = 0; i < 100; i++)
        {
            objet = UtilitaireApparitionPrefab.TentativeApparaitre(prefabCapsuleSimple);
        }
        // Assert
        Assert.IsNotNull(objet);
    }
    [Test]
    public void ApparaitreSiLibre_SceneDejaRemplie()
    {
        // Arrange
        for (int i = 0; i < 10000; i++)
        {
            UtilitaireApparitionPrefab.TentativeApparaitre(prefabCapsuleSimple);
        }
        GameObject objet = null;

        // Act
        for (int i = 0; i < 100; i++)
        {
            objet = UtilitaireApparitionPrefab.TentativeApparaitre(prefabCapsuleSimple);
        }

        // Assert
        Assert.IsNull(objet);
    }


}