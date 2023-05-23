using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using RichTextSubstringHelper;

namespace EasySubtitles
{
    public class SubtitlePlayer : MonoBehaviour
    {
        public enum Type
        {
            Instant,
            CharacterByCharacter
        }
        // Inspector fields
        [SerializeField] private TextAsset _subtitles;
        [SerializeField] private Type _type = Type.Instant;
        [SerializeField] private TMP_Text _text;

        [Header("Audio")]
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
            // if we have an audio source, we use it to get the time
            if (_audioSource != null && _audioSource.isPlaying)
            {
                time = _audioSource.time;
            }
            // otherwise we use an internal timer
            else
            {
                timer += Time.deltaTime;

                time = timer;
            }

            // if we reached the end of the subtitles, we stop
            if (time >= parsedSubtitles.Duration)
            {
                Stop();
                return;
            }

            // we get the subtitle at the current time
            UpdateText(parsedSubtitles.GetSubtitleAt(timer));
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

            if (_type == Type.Instant)
            {
                _text.text = subtitle.Text;
            }
            else if (_type == Type.CharacterByCharacter)
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
        public void Play(Type type)
        {
            if (_subtitles == null)
            {
                Debug.LogError("No subtitles assigned");
                return;
            }
            Play(_subtitles, type);
        }

        /// <summary>
        /// Plays the subtitles
        /// </summary>
        public void Play()
        {
            Play(_type);
        }

        /// <summary>
        /// Plays the subtitles using the given subtitles text and type
        /// </summary>
        public void Play(TextAsset subtitles, Type type)
        {
            Play(subtitles, type, _audioSource);
        }

        /// <summary>
        /// Plays the subtitles using the given subtitles text, type and audio source
        /// </summary>
        public void Play(TextAsset subtitles, Type type, AudioSource audioSource)
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

            _type = type;
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
