# EasySubtitles

EasySubtitles is a Unity library that allows you to easily implement subtitles using the SRT file format. It supports rich text as well as two display modes: instant and character by character.

## Features

- Load subtitles from SRT files or strings
- Display subtitles in a UI text element
- Use rich text tags to customize the appearance of subtitles
- Choose between instant or character by character display modes

## Installation

To install EasySubtitles, you can download the package from the [releases](https://github.com/ookii-tsuki/EasySubtitles/releases/)

## Usage

Add a `SubtitlePlayer` component to a GameObject in your scene then call the public method `Play()` from a script.
The subtitles, mode of display and audio source (optional) can be assigned in inspector or can be passed as arguments to the `Play()` method.

Example code:

```csharp
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
}
```

## Examples

Example SRT string:

```
1
00:00:01,440 --> 00:00:04,100
Senator, we're making
our <b>final</b> approach into {u}Coruscant{/u}.

2
00:00:04,700 --> 00:00:06,100
{b}Very good, {i}Lieutenant{/i}{/b}.

3
00:00:06,400 --> 00:00:09,800 X1:0 X2:0 Y1:-750 Y2:750
<font color="#fbff1c">Whose side is time on?</font>

4
00:00:10,000 --> 00:00:11,900 X1:-500 X2:500 Y1:-500 Y2:500
AHHHHHHHHHH

5
00:00:12,300 --> 00:00:14,000
[speaks Icelandic]

6
00:00:14,500 --> 00:00:17,600
[man 3] <i>♪The admiral
begins his expedition♪</i>
```

Using character by character mode:

![Charater by charater mode example](https://github.com/ookii-tsuki/EasySubtitles/blob/3c0c6751bdd03087b8927555cf944566ca1e4df8/ExampleImages/character_by_character.gif)

Using instant mode:

![Instant mode example](https://github.com/ookii-tsuki/EasySubtitles/blob/main/ExampleImages/instant.gif)

## References

`Dialogue_1.mp3` is from my game [Eerie Enigma](https://ookii-tsuki.itch.io/eerie-enigma).

This library uses [Unity3dRichTextHelper](https://github.com/majecty/Unity3dRichTextHelper) by [majecty](https://github.com/majecty) to parse rich text tags.

The example SRT subtitles is from [Lokalise](https://docs.lokalise.com/en/articles/5365539-subrip-srt)

## License
This library is licensed under [MIT License](https://github.com/ookii-tsuki/EasySubtitles/blob/main/LICENSE).
