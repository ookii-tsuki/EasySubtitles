using System.Collections;
using UnityEngine;
using TMPro;
using RichTextSubstringHelper;

namespace EasySubtitles
{
    public class SubtitlePlayer : MonoBehaviour
    {
        public enum Mode
        {
            Instant,
            CharacterByCharacter
        }
        // Inspector fields
        [Header("Subtitles")]
        [Tooltip("The subtitles file to play")]
        [SerializeField] private TextAsset _subtitles;
        [Tooltip("The mode of display")]
        [SerializeField] private Mode _mode = Mode.Instant;
        [Tooltip("The text component to display the subtitles")]
        [SerializeField] private TMP_Text _text;

        [Header("Audio")]
        [Tooltip("The audio source that will be played with the subtitles")]
        [SerializeField] private AudioSource _audioSource;

        // Properties
        public TextAsset Subtitles
        {
            get => _subtitles;
            set => _subtitles = value;
        }
        public bool IsPlaying { get; private set; }

        // Private fields
        private Subtitles parsedSubtitles;
        private float timer;
        private Subtitle currentSubtitle;

        // Update is called once per frame
        void Update()
        {
            if (!IsPlaying || parsedSubtitles == null)
                return;

            float time;
            float duration;
            // if we have an audio source, we use it to get the time
            if (_audioSource != null && _audioSource.isPlaying)
            {
                time = _audioSource.time;
                duration = _audioSource.clip.length;
            }
            // otherwise we use an internal timer
            else
            {
                timer += Time.deltaTime;

                time = timer;
                duration = parsedSubtitles.Duration;
            }

            // if we reached the end of the subtitles, we stop
            if (duration - time < 0.1f)
            {
                Stop();
                return;
            }

            // we get the subtitle at the current time
            UpdateText(parsedSubtitles.GetSubtitleAt(time));
        }

        /// <summary>
        /// Updates the text of the subtitle using the given subtitle
        /// </summary>
        void UpdateText(Subtitle subtitle)
        {
            if (subtitle == currentSubtitle)
                return;

            currentSubtitle = subtitle;

            _text.margin = new Vector4(subtitle.X1, subtitle.Y1, subtitle.X2, subtitle.Y2);

            if (_mode == Mode.Instant)
            {
                _text.text = subtitle.Text;
            }
            else if (_mode == Mode.CharacterByCharacter)
            {
                StartCoroutine(PlayCharacterByCharacter(subtitle));
            }
        }

        /// <summary>
        /// Plays the subtitle character by character
        /// </summary>
        IEnumerator PlayCharacterByCharacter(Subtitle subtitle)
        {
            _text.text = string.Empty;
            var text = subtitle.Text;
            var length = text.Length;
            var delayTime = subtitle.Duration * 0.3f; // 30% of the subtitle duration
            var characterDelay = new WaitForSeconds(delayTime / length);

            var maker = new RichTextSubStringMaker(text); // we use a helper class by majecty to get the rich text substring
        
            while (maker.IsConsumable())
            {
                maker.Consume();
                _text.text = maker.GetRichText();
                yield return characterDelay;
            }
        }

        /// <summary>
        /// Plays the subtitles using the given type
        /// </summary>
        public void Play(Mode mode)
        {
            if (_subtitles == null)
            {
                Debug.LogError("No subtitles assigned");
                return;
            }
            Play(_subtitles, mode);
        }

        /// <summary>
        /// Plays the subtitles
        /// </summary>
        public void Play()
        {
            Play(_mode);
        }

        /// <summary>
        /// Plays the subtitles using the given subtitles text and type
        /// </summary>
        public void Play(TextAsset subtitles, Mode mode)
        {
            Play(subtitles, mode, _audioSource);
        }

        /// <summary>
        /// Plays the subtitles using the given subtitles text, type and audio source
        /// </summary>
        public void Play(TextAsset subtitles, Mode mode, AudioSource audioSource)
        {
            if (subtitles != _subtitles)
            {
                _subtitles = subtitles;
                parsedSubtitles = new Subtitles(subtitles);
            }
            else
            {
                if (parsedSubtitles == null)
                {
                    parsedSubtitles = new Subtitles(subtitles);
                }
            }

            _mode = mode;
            timer = 0;

            _audioSource = audioSource;
            if (_audioSource != null)
            {
                _audioSource.Play();
            }

            IsPlaying = true;
        }

        /// <summary>
        /// Stops the subtitles
        /// </summary>
        public void Stop()
        {
            if (_audioSource != null)
            {
                _audioSource.Stop();
            }
            IsPlaying = false;
            _text.text = string.Empty;
        }
    }
}
