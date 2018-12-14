using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbientManager : MonoBehaviour {
    [SerializeField]
    private AudioClip[] _ambientSounds;
    [SerializeField]
    private int _maxPlayCounter;
    private int _playCounter;
    private AudioSource _audioSource;
    private void Awake()
    {
        _playCounter = Random.Range(1, _maxPlayCounter + 1);
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = _ambientSounds[Random.Range(0, _ambientSounds.Length)];
        _audioSource.loop = false;
        _audioSource.Play();
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            if (--_playCounter < 0)
            {
                _audioSource.clip = _ambientSounds[Random.Range(0, _ambientSounds.Length)];
                _playCounter = Random.Range(1, _maxPlayCounter + 1);
            }        
            _audioSource.Play();
        }
    }
}
