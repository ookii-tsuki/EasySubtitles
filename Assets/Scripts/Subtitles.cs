using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

/// <summary>
/// A list of subtitles
/// </summary>
public class Subtitles : List<Subtitle>
{
    /// <summary>
    /// The length in seconds of the subtitle file
    /// </summary>
    public float Duration => this[this.Count - 1].End;

    /// <summary>
    /// Creates a new instance of the Subtitles class
    /// </summary>
    /// <param name="textAsset">The subtitle file</param>
    public Subtitles(TextAsset textAsset)
    {
        if (textAsset == null)
        {
            throw new ArgumentNullException(nameof(textAsset));
        }
        Parse(textAsset.text);
    }

    /// <summary>
    /// Creates a new instance of the Subtitles class
    /// </summary>
    /// <param name="subfile">The subtitle file</param>
    public Subtitles(string subfile)
    {
        Parse(subfile);
    }

    /// <summary>
    /// Parses the subtitle file into a list of subtitles
    /// see https://en.wikipedia.org/wiki/SubRip#SubRip_text_file_format for reference
    /// </summary>
    private void Parse(string subfile)
    {
        // Split the file into blocks of index, time and text
        var sublines = subfile.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
        
        foreach (var line in sublines)
        {
            // Parse the block into its properties
            RegexOptions options = RegexOptions.Multiline;
            var blockProperties = Regex.Match(line, @"^(\d+)\r?\n(\d{2}:\d{2}:\d{2},\d{3})\s-->\s(\d{2}:\d{2}:\d{2},\d{3})(?:\sX1:(-?\d+)\sX2:(-?\d+)\sY1:(-?\d+)\sY2:(-?\d+))?\r?\n([\S\s]+)$", options);
            if (!blockProperties.Success)
            {
                Debug.LogError("Invalid subtitle block: " + line);
                continue;
            }

            // Parse the properties into variables

            var index = int.Parse(blockProperties.Groups[1].Value); // Index
            var start = ParseTime(blockProperties.Groups[2].Value); // start time
            var end = ParseTime(blockProperties.Groups[3].Value); // end time
            var text = FixFormatting(blockProperties.Groups[8].Value.Trim()); // Text
            var x1 = !string.IsNullOrEmpty(blockProperties.Groups[4].Value) ? int.Parse(blockProperties.Groups[4].Value) : 0; // X1
            var x2 = !string.IsNullOrEmpty(blockProperties.Groups[5].Value) ? int.Parse(blockProperties.Groups[5].Value) : 0; // X2
            var y1 = !string.IsNullOrEmpty(blockProperties.Groups[6].Value) ? int.Parse(blockProperties.Groups[6].Value) : 0; // Y1
            var y2 = !string.IsNullOrEmpty(blockProperties.Groups[7].Value) ? int.Parse(blockProperties.Groups[7].Value) : 0; // Y2

            // Add the subtitle to the list
            Add(new Subtitle(index, start, end, x1, x2, y1, y2, text));
        }
    }

    /// <summary>
    /// Parses a time string into a float
    /// </summary>
    private float ParseTime(string time)
    {
        var timeParts = time.Split(new[] { ':', ',' }, StringSplitOptions.RemoveEmptyEntries);
        var hours = int.Parse(timeParts[0]);
        var minutes = int.Parse(timeParts[1]);
        var seconds = int.Parse(timeParts[2]);
        var milliseconds = int.Parse(timeParts[3]);
        return hours * 3600 + minutes * 60 + seconds + milliseconds / 1000f;
    }

    /// <summary>
    /// Fixes the formatting of the text
    /// </summary>
    private string FixFormatting(string text)
    {
        // replacing the { and } with < and > so that the text can be parsed by the TextMeshPro component
        text = Regex.Replace(text, @"{(\w)}", "<$1>");
        text = Regex.Replace(text, @"{(/\w)}", "<$1>");
        text = Regex.Replace(text, @"<font color=\x22(.*)\x22>([\S\s]+)</font>", "<color=$1>$2</color>");
        return text;
    }

    /// <summary>
    /// Returns the subtitles that are active at the given time
    /// </summary>
    /// <param name="time">The time to check</param>
    /// <returns>The subtitles that are active at the given time</returns>
    public Subtitle GetSubtitleAt(float time)
    {
        var subtitle = this.Find(subtitle => subtitle.Start <= time && subtitle.End >= time);

        return subtitle ?? Subtitle.Empty;
    }
}

/// <summary>
/// A single subtitle line
/// </summary>
public class Subtitle
{
    static Subtitle _empty;
    public static Subtitle Empty => _empty ?? (_empty = new Subtitle(0, 0, 0, 0, 0, 0, 0, string.Empty));
    public int Index { get; }
    public float Start { get; }
    public float End { get; }
    public float Duration { get; }
    public int X1 { get; }
    public int X2 { get; }
    public int Y1 { get; }
    public int Y2 { get; }
    public string Text { get; }

    public Subtitle(int index, float start, float end, int x1, int x2, int y1, int y2, string text)
    {
        Index = index;
        Start = start;
        End = end;
        Duration = end - start;
        X1 = x1;
        X2 = x2;
        Y1 = y1;
        Y2 = y2;
        Text = text;
    }

    public override string ToString()
    {
        return $"Index: {Index}\nText: {Text}\nStart: {Start}\nEnd: {End}\nDuration: {Duration}\nX1: {X1}\nX2: {X2}\nY1: {Y1}\nY2: {Y2}";
    }
}