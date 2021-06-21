using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SongPlayer : MonoBehaviour
{
    private bool hasStarted = false;

    public AudioMixerSnapshot mainMenu;
    //public AudioClip lobby;
    public AudioMixerSnapshot inGame;

    public AudioSource source;

    public AudioMixer mainMixer;

    public Slider volumeSlider;

    public float chosenVolume = 1f;
    public float volume = 0f;

    void Start()
    {
        volume = -80f;
        mainMixer.SetFloat("Volume", volume);

        if (PlayerPrefs.HasKey("Volume"))
        {
            chosenVolume = PlayerPrefs.GetFloat("Volume");
        } else
        {
            chosenVolume = -20f;
        }
        volumeSlider.value = chosenVolume;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted)
        {
            volume = chosenVolume;
        } else if (!hasStarted)
        {
            if (volume < chosenVolume)
            {
                volume += 0.8f;
            } else if (volume >= chosenVolume)
            {
                hasStarted = true;
                volume = chosenVolume;
            }
        }

        chosenVolume = volumeSlider.value;
        PlayerPrefs.SetFloat("Volume", chosenVolume);

        mainMixer.SetFloat("Volume", volume);

        if (!GameManager.instance.hasStarted)
        {
            mainMenu.TransitionTo(2.0f);
            //if (source.clip != mainMenu)
            //{
            //    source.Stop();
            //}

            //source.clip = mainMenu;
            //if (!source.isPlaying)
            //{
            //    source.PlayOneShot(mainMenu);
            //}

            
        }
        else if (GameManager.instance.hasStarted)
        {
            inGame.TransitionTo(2.0f);
            //if (source.clip != inGame)
            //{
            //    source.Stop();
            //}

            //source.clip = inGame;
            //if (!source.isPlaying)
            //{
            //    source.PlayOneShot(inGame);
            //}
        }



        //if (volume < chosenVolume)
        //{
        //    volume += 0.0025f;
        //} else if (volume > chosenVolume)
        //{
        //    volume -= 0.0025f;
        //}
        //source.volume = volume;
    }

    public void SetVolume(float vol)
    {
        chosenVolume = vol;
    }
}
