using UnityEngine;

namespace GameDevWithMarco.DataSO
{
    [CreateAssetMenu(fileName = "New Sound Data", menuName = "Scriptable Objects/SoundSO")]
    public class SoundSO : ScriptableObject
    {
        public float minPitchValue;
        public float maxPitchValue;
        public AudioClip clipToUse;
        public float soundVolume;
        
    }
}
