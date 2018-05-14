using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MenuScript : MonoBehaviour {

    public AudioMixer AM;

    public void playgame(string Level) {
        SceneManager.LoadScene(Level);
    }
    public void Quit() {
        Application.Quit();
    }
    public void SetVolume(float Volume) {
        AM.SetFloat("Volume",Volume);
    }

}
