using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BSS {
    public class SoundSystem : Singleton<SoundSystem> {
        [Range(1, 10)]
        public int playMaxCount = 10;
        [Range(0f, 1f)]
        public float baseBgmVolume = 1f;
        [Range(0f, 1f)]
        public float baseEffectVolume = 1f;
        public List<AudioClip> clips = new List<AudioClip>();

        private AudioSource bgmSource;
        private List<AudioSource> audioSources = new List<AudioSource>();

        private bool isInit;

        public void Awake() {
            Initialize();
        }
        /// <summary>
        /// 현재 재생중인 배경 음악을 가져옵니다.
        /// </summary>
        public static AudioClip GetPlayingBgmClip() {
            instance.Initialize();
            if (!instance.bgmSource.isPlaying) return null;
            return instance.bgmSource.clip;
        }

        /// <summary>
        /// 배경 음악을 설정하고 재생합니다.
        /// </summary>
        public static void SetBgm(AudioClip clip, float volume = 1f) {
            instance.Initialize();
            if (instance.bgmSource.isPlaying) {
                if (clip.name == instance.bgmSource.clip.name) return;
                instance.bgmSource.Stop();
            }
            instance.bgmSource.volume = instance.baseBgmVolume * volume;
            instance.bgmSource.clip = clip;
            instance.bgmSource.Play();
        }

        /// <summary>
        /// 배경 음악을 설정하고 재생합니다.
        /// </summary>
        public static void SetBgm(string clipName, float volume = 1f) {
            instance.Initialize();
            var clip = instance.FindClip(clipName);
            SetBgm(clip, volume);
        }

        /// <summary>
        /// 배경 음악의 볼륨을 조절합니다.
        /// </summary>
        public static void ChangeBgmVolume(float volume) {
            instance.baseBgmVolume = volume;
            instance.bgmSource.volume = instance.baseBgmVolume * volume;
        }

        /// <summary>
        /// 모든 효과음의 볼륨을 조절합니다.
        /// </summary>
        public static void ChangeEffectVolume(float volume) {
            instance.baseEffectVolume = volume;
            instance.audioSources.ForEach(x => x.volume = instance.baseEffectVolume * volume);
        }

        /// <summary>
        /// 배경 음악을 재개합니다.
        /// </summary>
        public static void ResumeBgm() {
            instance.Initialize();
            instance.bgmSource.Play();
        }
        /// <summary>
        /// 배경 음악을 멈춥니다.
        /// </summary>
        public static void PauseBgm() {
            instance.Initialize();
            instance.bgmSource.Pause();
        }

        /// <summary>
        /// 사운드를 1회 재생합니다.
        /// </summary>
        public static AudioSource PlayOnce(AudioClip clip, float volume = 1f) {
            instance.Initialize();
            var audioSource = instance.audioSources.Find(x => !x.isPlaying);
            if (audioSource == null) return null;
            audioSource.loop = false;
            audioSource.clip = clip;
            audioSource.volume = instance.baseEffectVolume * volume;
            audioSource.Play();
            return audioSource;
        }
        /// <summary>
        /// 사운드를 1회 재생합니다.
        /// </summary>
        public static AudioSource PlayOnce(string clipName, float volume = 1f) {
            instance.Initialize();
            var clip = instance.FindClip(clipName);
            return PlayOnce(clip, volume);
        }
        /// <summary>
        /// 사운드를 반복 재생합니다.
        /// </summary>
        public static AudioSource PlayLoop(AudioClip clip, float volume = 1f) {
            instance.Initialize();
            var audioSource = instance.audioSources.Find(x => !x.isPlaying);
            if (audioSource == null) return null;
            audioSource.loop = true;
            audioSource.clip = clip;
            audioSource.volume = instance.baseEffectVolume * volume;
            audioSource.Play();
            return audioSource;
        }
        /// <summary>
        /// 사운드를 반복 재생합니다.
        /// </summary>
        public static AudioSource PlayLoop(string clipName, float volume = 1f) {
            instance.Initialize();
            var clip = instance.FindClip(clipName);
            return PlayLoop(clip, volume);
        }
        /// <summary>
        /// 사운드를 일정 시간동안 반복 재생합니다.
        /// </summary>
        public static void PlayLoopInTime(AudioClip clip, float loopTime, float volume = 1f) {
            instance.Initialize();
            var source = PlayLoop(clip, volume);
            instance.StartCoroutine(instance.CoExcuteAfterSeconds(loopTime, () => {
                if (source == null || !source.isPlaying || source.clip != clip) return;
                source.Stop();
            }));
        }
        /// <summary>
        /// 사운드를 일정 시간동안 반복 재생합니다.
        /// </summary>
        public static void PlayLoopInTime(string clipName, float loopTime, float volume = 1f) {
            instance.Initialize();
            var clip = instance.FindClip(clipName);
            PlayLoopInTime(clip, loopTime, volume);
        }
        /// <summary>
        /// 사운드를 특정 조건이 만족하기 전까지 반복 재생합니다.
        /// </summary>
        public static void PlayLoopInCondition(AudioClip clip, System.Func<bool> endCondition, float volume = 1f) {
            instance.Initialize();
            var source = PlayLoop(clip, volume);
            instance.StartCoroutine(instance.CoExcuteAfterCondition(endCondition, () => {
                if (source == null || !source.isPlaying || source.clip != clip) return;
                source.Stop();
            }));
        }
        /// <summary>
        /// 사운드를 특정 조건이 만족하기 전까지 반복 재생합니다.
        /// </summary>
        public static void PlayLoopInCondition(string clipName, System.Func<bool> endCondition, float volume = 1f) {
            instance.Initialize();
            var clip = instance.FindClip(clipName);
            PlayLoopInCondition(clip, endCondition, volume);
        }



        IEnumerator CoExcuteAfterSeconds(float seconds, Action act) {
            yield return new WaitForSeconds(seconds);
            act?.Invoke();
        }
        IEnumerator CoExcuteAfterCondition(Func<bool> condition, Action act) {
            yield return new WaitUntil(condition);
            act?.Invoke();
        }


        private AudioSource CreateChild() {
            var obj = new GameObject("Sound Player");
            obj.transform.SetParent(transform);
            var source = obj.AddComponent<AudioSource>();
            return source;
        }

        private void Initialize() {
            if (isInit) return;
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
            for (int i = 0; i < playMaxCount; i++) {
                audioSources.Add(CreateChild());
            }
            isInit = true;
        }

        private AudioClip FindClip(string clipName) {
            var clip= instance.clips.Find(x => x.name == clipName);
            if (clip == null) throw new System.Exception($"Clip is not exist. ({clipName})");
            return clip;
        }
    }
}
