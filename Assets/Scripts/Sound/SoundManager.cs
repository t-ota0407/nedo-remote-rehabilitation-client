using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string WIND_CHIME_AUDIO_NAME = "WindChime";
    private const string COIN_AUDIO_NAME = "Coin";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayWindChimeSE()
    {
        PlaySE(WIND_CHIME_AUDIO_NAME);
    }

    public void PlayCoinSE()
    {
        PlaySE(COIN_AUDIO_NAME);
    }

    private void PlaySE(string audioName)
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        foreach (var audioSource in audioSources)
        {
            if (audioSource.clip.name.Equals(audioName))
            {
                audioSource.Play();
            }
        }
    }
}
