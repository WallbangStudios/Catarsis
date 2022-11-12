using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

    public AudioMixer AM;

    public AudioSource SelectSound;
    public float Delay;

    public SpriteRenderer BrightnessLight;

    public Slider SVolume, SBrightness;

    float MVolume;
    int MBrightness;

    void Start()
    {
        float vol = PlayerPrefs.GetFloat("Volume");
        int Bright = PlayerPrefs.GetInt("Brightness");
        SetVolume(vol);
        SetBrightness(Bright);
        SVolume.value = vol;
        SBrightness.value = Bright;
    }

    public void PlaySelectSound(){
        SelectSound.Play();
    }
    public void playgame(string Level) {
        SceneManager.LoadScene(Level);
    }
    public void Quit() {
        PlayerPrefs.Save();
        Application.Quit();
    }
    public void SetVolume(float Volume) {
        AM.SetFloat("Volume",Volume);
        MVolume = Volume;
    }

    public void SetBrightness(float Brightness) {
        var Ambient = BrightnessLight.color;
        float ColorBrighness = Brightness / 255;
        BrightnessLight.color = new Color(Ambient.r, Ambient.g, Ambient.b, ColorBrighness);
        MBrightness = (int)Brightness;
    }

    public void SetConfigs() {
        PlayerPrefs.SetFloat("Volume", MVolume);
        PlayerPrefs.SetInt("Brightness", MBrightness);
        PlayerPrefs.Save();
    }
}
