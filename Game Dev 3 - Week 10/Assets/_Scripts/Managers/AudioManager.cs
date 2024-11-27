using GameDevWithMarco.DataSO;
using UnityEngine;

namespace GameDevWithMarco.Managers
{
    public class AudioManager : Singleton<AudioManager>
    {
        /// <summary>
        /// This script will drive anything related to Audio
        /// </summary>

        [SerializeField] AudioClip backgroundMusic;
        [SerializeField] SoundSO goodPickupSoundData;
        [SerializeField] SoundSO badPickupSoundData;
        [SerializeField] SoundSO dashSoundData;
        [SerializeField] SoundSO lifeSoundData;
        [SerializeField] AudioSource audioSource_Music;
        [SerializeField] AudioSource audioSource_Sounds;

        // Start is called before the first frame update
        void Start()
        {
            if (backgroundMusic != null)
            {
                PlayBackgroundMusic();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (backgroundMusic != null)
            {
                audioSource_Music.volume += 0.1f * Time.deltaTime;
                if (audioSource_Music.volume >= 0.2f)
                {
                    audioSource_Music.volume = 0.2f;
                }
            }
        }

        public void PlaySounds(float lowPitchRange, float highPitchRange, AudioClip clipToPlay, float volume)
        {
            audioSource_Sounds.pitch = Random.Range(lowPitchRange, highPitchRange);
            audioSource_Sounds.PlayOneShot(clipToPlay);
            audioSource_Sounds.volume = volume;
        }

        public void GoodPickupSound()
        {
            PlaySounds(goodPickupSoundData.minPitchValue, goodPickupSoundData.maxPitchValue,
                goodPickupSoundData.clipToUse, goodPickupSoundData.soundVolume);
        }
        public void BadPickupSound()
        {
            PlaySounds(badPickupSoundData.minPitchValue, badPickupSoundData.maxPitchValue,
                badPickupSoundData.clipToUse, badPickupSoundData.soundVolume);
        }
        public void PlayBackgroundMusic()
        {

            audioSource_Music.volume = 0f;
            audioSource_Music.clip = backgroundMusic;
            audioSource_Music.Play();
            audioSource_Music.loop = true;
        }
        public void Dash()
        {
            PlaySounds(dashSoundData.minPitchValue, dashSoundData.maxPitchValue,
                dashSoundData.clipToUse, dashSoundData.soundVolume);
        }
        public void LifePickupSound()
        {
            PlaySounds(lifeSoundData.minPitchValue, lifeSoundData.maxPitchValue,
                lifeSoundData.clipToUse, lifeSoundData.soundVolume);
        }
    }
}
