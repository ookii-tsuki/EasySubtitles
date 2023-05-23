using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasySubtitles;

public class TestScript : MonoBehaviour
{
    public TextAsset _subtitles;
    [SerializeField] SubtitlePlayer subtitlesPlayer;
    [SerializeField] SubtitlePlayer.Type type = SubtitlePlayer.Type.CharacterByCharacter;
    // Start is called before the first frame update
    void Start()
    {
        subtitlesPlayer.Play(_subtitles, type);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (subtitlesPlayer.IsPlaying)
                subtitlesPlayer.Stop();
            else
                subtitlesPlayer.Play(_subtitles, type);
        }
    }
}
