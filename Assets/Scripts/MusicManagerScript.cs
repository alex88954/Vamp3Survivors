using UnityEngine;
using UnityEngine.UI;
// Script pour gérer la musique
public class MusicManagerScript : MonoBehaviour
{
    private AudioSource music;
    public static MusicManagerScript instance { get; private set; }
    // Assurer que la musique ne soit pas détruite au changement de scène
    // Singleton pour éviter les doublons de musique
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
            music = GetComponent<AudioSource>();
            music.volume = PlayerPrefs.GetFloat("MusicVolume", 0.8f);
            DontDestroyOnLoad(gameObject);
        }
    }
    // Getter pour le volume de la musique
    public float GetVolume()
    {
        return music.volume;
    }
    // Changer le volume et l'enregistrer
    public void SetVolume(float value)
    {
        music.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", music.volume);
        PlayerPrefs.Save();
    }
}
