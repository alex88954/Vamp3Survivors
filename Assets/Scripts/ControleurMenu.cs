using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ControleurMenu : MonoBehaviour
{
    [SerializeField] private GameObject[] ArrierePlans;
    [SerializeField] private Slider musicSlider;
    private MusicManagerScript musicManagerScript;
    private int arrierePlanActif = 0;

    private void Start()
    {
        StartCoroutine(Animation());

        ArrierePlans[0].SetActive(true);
        for (int i = 1; i < ArrierePlans.Length; i++)
            ArrierePlans[i].SetActive(false);

        // Initialiser le slider de musique avec la valeur actuelle du volume
        musicManagerScript = MusicManagerScript.instance;
        musicSlider.value = musicManagerScript.GetVolume();
        VolumeChanged();
    }

    public IEnumerator Animation()
    {
        while (true)
        {
            int nouvelArrierePlan = 0;
            do
            {
                nouvelArrierePlan = Random.Range(0, ArrierePlans.Length);
            } while (nouvelArrierePlan == arrierePlanActif);

            ArrierePlans[arrierePlanActif].SetActive(false);
            ArrierePlans[nouvelArrierePlan].SetActive(true);

            arrierePlanActif = nouvelArrierePlan;

            // Arri�re-plan random
            yield return new WaitForSeconds(Random.value * 0.5f);
        }
    }

    public void DemarrerFacile()
    {
        ParametresJeu.Instance.ModeFacile();
        Demarrer();
    }

    public void DemarrerDifficile()
    {
        ParametresJeu.Instance.ModeDifficile();
        Demarrer();
    }

    private void Demarrer()
    {
        SceneManager.LoadScene("Jeu");
    }

    // Mettre à jour le volume de la musique lorsque le slider change
    public void VolumeChanged()
    {
        musicManagerScript?.SetVolume(musicSlider.value);
    }

    public void Quitter()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}