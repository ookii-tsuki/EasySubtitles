using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlePlayer : MonoBehaviour
{
    [SerializeField] private TextAsset _subtitles;

    public TextAsset Subtitles
    {
        get => _subtitles;
        set => _subtitles = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        Subtitles subtitles = new Subtitles(_subtitles);

        foreach (var subtitle in subtitles)
        {
            Debug.Log(subtitle.ToString());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
