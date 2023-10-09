/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicClass : MonoBehaviour
{
    private AudioSource _audioSource;
    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicClass : MonoBehaviour
{
	private AudioSource _audioSource;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		_audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		if (scene.name == "MainMenu")
		{
			StopMusic();
		}
		else
		{
			PlayMusic();
		}
	}

	public void PlayMusic()
	{
		if (!_audioSource.isPlaying)
		{
			_audioSource.Play();
		}
	}

	public void StopMusic()
	{
		_audioSource.Stop();
	}
}
