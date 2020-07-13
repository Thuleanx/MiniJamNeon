
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[System.Serializable]
	public class Sound {
		public string name;
		public AudioClip clip;

		[Range(0f, 1f)]
		public float volume;

		[Range(0.1f, 3f)]
		public float pitch;

		public bool loop;

		[HideInInspector]
		public AudioSource source;
	}

	public static AudioManager Instance;

	[SerializeField] List<Sound> sounds = new List<Sound>();
	Dictionary<string, int> soundIndex = new Dictionary<string, int>();

	void Awake() {
		if (Instance == null)
			Instance = this;
		else Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		for (int i = 0; i < sounds.Count; i++)
			soundIndex[sounds[i].name] = i;	
		foreach (Sound s in sounds) {
			GameObject audioObject = new GameObject(s.name, typeof(AudioSource));
			audioObject.transform.parent = transform;
			s.source = audioObject.GetComponent<AudioSource>();
			s.source.volume = s.volume;
			s.source.clip = s.clip;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}

		if (soundIndex.ContainsKey("Music")) {
			sounds[soundIndex["Music"]].source.Play();
		}
	}

	public void Play(string name) {
		if (soundIndex.ContainsKey(name)) {
			sounds[soundIndex[name]].source.Play();
			print("HI");
		}
	}

	void Start() {
		Play("Background");
	}
}