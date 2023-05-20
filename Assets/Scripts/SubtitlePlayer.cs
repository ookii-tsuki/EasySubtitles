using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtitlePlayer : MonoBehaviour
{
    [SerializeField] private TextAsset _subtitles;
    // Start is called before the first frame update
    void Start()
    {
        Subtitles subtitles = new Subtitles(_subtitles);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
