using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class clickSound : MonoBehaviour
{   //intention was to make this reuasable for buttons throughout game so this might be good to put for combat UI buttons

    [SerializeField] public AudioClip sound;
    private Button button {get{return GetComponent<Button>();}}
    private AudioSource audioSource { get { return GetComponent<AudioSource>(); } }
    void Start()
    {
        gameObject.AddComponent<AudioSource>();
        audioSource.clip = sound;
        audioSource.playOnAwake = false;

        button.onClick.AddListener(() => PlaySound());
    }

   
    void PlaySound()
    {
        audioSource.PlayOneShot(sound);
    }
}
