using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySubtitles;

public class TestScript : MonoBehaviour
{
    public TextAsset _subtitles;
    [SerializeField] SubtitlePlayer subtitlesPlayer;
    [SerializeField] SubtitlePlayer.Mode mode = SubtitlePlayer.Mode.CharacterByCharacter;
    [SerializeField] AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        subtitlesPlayer.Play(_subtitles, mode, audioSource);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (subtitlesPlayer.IsPlaying)
                subtitlesPlayer.Stop();
            else
                subtitlesPlayer.Play(_subtitles, mode, audioSource);
        }
    }
}
