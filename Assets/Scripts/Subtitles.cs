using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Subtitles : List<Subtitle>
{
    public float Length => this[this.Count - 1].To;

    public Subtitles(TextAsset textAsset)
    {
        Parse(textAsset.text);
    }
    public Subtitles(string subfile)
    {
        Parse(subfile);
    }

    private void Parse(string subfile)
    {
        var sublines = Regex.Split(subfile, @"^(?=$\n\d)");
        
        foreach (var line in sublines)
            Debug.Log(line);
    }
}

public class Subtitle
{
    public int Index { get; }
    public float From { get; }
    public float To { get; }
    public float Duration { get; }
    public string Text { get; }

    public Subtitle(int index, float from, float to, string text)
    {
        Index = index;
        From = from;
        To = to;
        Duration = to - from;
        Text = text;
    }
}