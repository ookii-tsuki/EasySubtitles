# EasySubtitles

EasySubtitles is a Unity library that allows you to easily implement subtitles using the SRT file format. It supports rich text as well as two display modes: instant and character by character.

## Features

- Load subtitles from SRT files or strings
- Display subtitles in a UI text element
- Use rich text tags to customize the appearance of subtitles
- Choose between instant or character by character display modes

## Installation

To install EasySubtitles, you can download the package from the [releases](https://github.com/yourusername/EasySubtitles/releases)

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

## References

`Dialogue_1.mp3` is from my game [Eerie Enigma](https://ookii-tsuki.itch.io/eerie-enigma).

This library uses [Unity3dRichTextHelper](https://github.com/majecty/Unity3dRichTextHelper) by [majecty](https://github.com/majecty) to parse rich text tags.

## License
This library is licensed under [MIT License](https://github.com/ookii-tsuki/EasySubtitles/blob/main/LICENSE).
