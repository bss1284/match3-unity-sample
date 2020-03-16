using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


namespace BSS {
#if E7_NATIVE_AUDIO
    /// E7 Native Audio Asset Required
    /// http://exceed7.com/native-audio
    using E7.Native;
    public class NativeSoundSystem : Singleton<NativeSoundSystem> {
        [Range(0f, 1f)]
        public float baseVolume = 1f;
        [SerializeField]
        public List<AudioClip> clips = new List<AudioClip>();

        private Dictionary<AudioClip, NativeAudioPointer> pointers = new Dictionary<AudioClip, NativeAudioPointer>();

        private NativeAudioController nativeAudioController;


        private void Awake() {
            Initialize();
        }


        public static void PlayOnce(AudioClip clip, float volume = 1f) {
            if (instance.NotRealDevice()) {
                SoundSystem.PlayOnce(clip);
                return;
            }
            if (!instance.pointers.ContainsKey(clip)) {
                instance.pointers[clip] = NativeAudio.Load(clip);
            }
            var option = new NativeAudio.PlayOptions();
            option.volume = instance.baseVolume * volume;
            instance.nativeAudioController = instance.pointers[clip].Play(option);
        }
        public static void PlayOnce(string clipName, float volume = 1f) {
            var clip = instance.clips.Find(x => x.name == clipName);
            PlayOnce(clip, volume);
        }

        private void Initialize() {
    #if UNITY_EDITOR
            //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
            Debug.Log("Please try this in a real device!");
    #else
        #if UNITY_ANDROID
        var audioInfo = NativeAudio.GetDeviceAudioInformation();
        Debug.Log("Android audio information : " + audioInfo);
        #endif
        NativeAudio.Initialize(new NativeAudio.InitializationOptions { androidAudioTrackCount = 3 });
    #endif
        }

        private bool NotRealDevice() {
    #if UNITY_EDITOR
            //You should have a fallback to normal AudioSource playing in your game so you can also hear sounds while developing.
            return true;
    #else
        return false;
    #endif
        }
    }
#endif
}