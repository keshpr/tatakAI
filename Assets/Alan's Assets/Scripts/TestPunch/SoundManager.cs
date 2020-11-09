using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip Step, Punch, Land, Swipe;
    public AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Prototype for method of handling sound effects in game
    //Still has several flaws:
    // - use can result in sound effects played multiple times before the first call finishes
    public void PlayAudioClip(string clip)
    {
        switch(clip)
        {
            case "step":
                source.PlayOneShot(Step);
                break;
            case "land":
                source.PlayOneShot(Land);
                break;
            case "punch":
                source.PlayOneShot(Punch);
                break;
            case "swipe":
                source.PlayOneShot(Swipe);
                break;
            default:
                break;
        }
    }
}
