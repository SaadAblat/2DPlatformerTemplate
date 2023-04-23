using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;

public class AudioManager : MonoBehaviour
{
	public static AudioManager instance;

	public AudioMixerGroup mixerGroup;

	public Sound[] sounds;


	void Awake()
	{
		if (instance != null)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}

		foreach (Sound s in sounds)
		{
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.loop = s.loop;

			s.source.outputAudioMixerGroup = mixerGroup;
		}
	}

	public void Play(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}

		s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
		s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

		s.source.Play();
	}
	public bool IsPlaying(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		return s.source.isPlaying;
	}
	public void SetSoundVolume(string sound, float volume)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.volume = volume;

	}

	public void Stop(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		s.source.Stop();

	}
	public void StopAllSounds()
	{
		foreach (Sound snd in sounds)
		{
			snd.source.Stop();

		}
	}
	public void FadeoutAllSounds()
    {
		foreach (Sound snd in sounds)
		{
			StartCoroutine(StartFade(snd.source, 2f, 0));
			StartCoroutine(StopAudioAfterTime(snd.source, 2f));
		}
	}
	public void FadeoutSound(string sound)
	{
		Sound s = Array.Find(sounds, item => item.name == sound);
		StartCoroutine(StartFade(s.source, 2f, 0));
		StartCoroutine(StopAudioAfterTime(s.source, 2f));
	}
	public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
	{
		float currentTime = 0;
		float start = audioSource.volume;
		while (currentTime < duration)
		{
			currentTime += Time.deltaTime;
			audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
			yield return null;
		}
		yield break;
	}
	IEnumerator StopAudioAfterTime(AudioSource source, float time)
    {
		yield return new WaitForSeconds(time);
		source.Stop();
		StopAllCoroutines();
	}
}
