using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	public Sound[] Sounds;

	// Use this for initialization
	void Awake(){

		foreach (Sound s in Sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.Clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			s.source.loop = s.loop;
		}
	}

	void Update(){
		
		foreach (Sound s in Sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.Clip;

			s.source.volume = s.volume;
			s.source.pitch = s.pitch;

			s.source.loop = s.loop;
		}
	}
	
	public void Play(string name){
		try
		{
			Sound s = Array.Find(Sounds, sound => sound.name == name);
			s.source.Play();
		}
		catch (System.Exception e)
		{
			Debug.LogException(e);
		}
		
	}
}
[System.Serializable]
public class Sound{

	public string name;
	public AudioClip Clip;
	
	[Range(0,1)]
	public float volume;
	[Range(.1f,3f)]
	public float pitch;
	
	public bool loop;

	[HideInInspector]
	public AudioSource source;
}