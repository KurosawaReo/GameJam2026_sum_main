/*
   - KR.Sound - (Unity)
   ver.2026/08/15
*/
using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// サウンド用の追加機能.
/// </summary>
namespace KR.Unity.Sound
{
    /// <summary>
    /// サウンドデータ.
    /// </summary>
    [Serializable]
    public struct SoundData
    {
        public string    name;  //登録名.
        public AudioClip audio; //コンポーネント.
    }

    /// <summary>
    /// [継承想定]
    /// サウンド管理機能.
    /// 追加で欲しい機能があれば、継承して自由に機能追加できる.
    /// </summary>
    public class SoundMngKR : MonoBehaviour
    {
        public static SoundMngKR Inst; //実体を入れる用.

        [Header("- SoundMngKR -")]
        [Space(4)]
        [SerializeField] protected AudioSource audioSourceBgm;
        [SerializeField] protected AudioSource audioSourceSe;
        [Space(4)]
        [SerializeField] protected List<SoundData> bgmData; //BGMデータ配列.
        [SerializeField] protected List<SoundData> seData;  //SE データ配列.

        protected Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>(); //BGM保存用.
        protected Dictionary<string, AudioClip> seClips  = new Dictionary<string, AudioClip>(); //SE 保存用.

        /// <summary>
        /// SoundMngKRの初期化.
        /// </summary>
        public void InitSoundMngKR()
        {
            if (Inst == null)
            {
                Inst = this;                   //実体を保存.
                DontDestroyOnLoad(gameObject); //Scene移動しても消さずに残す.

                RegistSound(); //サウンド登録.
            }
            else
            {
                Destroy(gameObject); //2つ目以降は消去.
            }
        }

        /// <summary>
        /// サウンド登録.
        /// </summary>
        private void RegistSound()
        {
            //BGM登録.
            foreach(var i in bgmData)
            {
                bgmClips.Add(i.name, i.audio);
            }
            //SE登録.
            foreach (var i in seData)
            {
                seClips.Add(i.name, i.audio);
            }
        }

        /// <summary>
        /// BGM再生.
        /// </summary>
        /// <param name="name">BGM登録名</param>
        public void PlayBGM(string name, bool isLoop)
        {
            //Dictionaryから値を取得.
            if (bgmClips.TryGetValue(name, out var bgm)) {
                audioSourceBgm.clip = bgm;    //取得したサウンドを入れる.
                audioSourceBgm.loop = isLoop; //ループ設定.
                audioSourceBgm.Play();        //再生.
            }
        }
        /// <summary>
        /// SE再生.
        /// </summary>
        /// <param name="name">SE登録名</param>
        public void PlaySE(string name)
        {
            //Dictionaryから値を取得.
            if (seClips.TryGetValue(name, out var se)) {
                audioSourceSe.PlayOneShot(se); //取得したサウンドを再生.
            }
        }

        /// <summary>
        /// BGMを停止.
        /// </summary>
        public void StopBGM()
        {
            audioSourceBgm.Stop();
        }
        /// <summary>
        /// SEを停止.
        /// </summary>
        public void StopSE()
        {
            audioSourceSe.Stop();
        }

        /// <summary>
        /// BGM音量取得.
        /// </summary>
        public float GetVolumeBGM() => audioSourceBgm.volume;
        /// <summary>
        /// SE音量取得.
        /// </summary>
        public float GetVolumeSE() => audioSourceSe.volume;

        /// <summary>
        /// BGM音量設定.
        /// </summary>
        public void SetVolumeBGM(float volume)
        {
            audioSourceBgm.volume = Mathf.Clamp(volume, 0f, 1f); //0.0～1.0の範囲で設定.
        }
        /// <summary>
        /// SE音量設定.
        /// </summary>
        public void SetVolumeSE(float volume)
        {
            audioSourceSe.volume = Mathf.Clamp(volume, 0f, 1f); //0.0～1.0の範囲で設定.
        }

        /// <summary>
        /// BGMの再生位置を変更.
        /// </summary>
        /// <param name="time">再生位置(秒)</param>
        public void SetTimeBGM(float time)
        {
            //0～曲の長さの範囲にする.
            float clampTime = Mathf.Clamp(
                time,
                0.0f,
                audioSourceBgm.clip.length
            );
            //再生位置を設定.
            audioSourceBgm.time = clampTime;
        }

        /// <summary>
        /// BGMが最後まで再生されたか取得.
        /// </summary>
        public bool IsBGMFinished()
        {
            //BGMが存在しなければ終了扱いにしない.
            if (audioSourceBgm.clip == null)
            {
                return false;
            }
            //ループ中なら終了しない.
            if (audioSourceBgm.loop)
            {
                return false;
            }

            //再生が終了しているか確認.
            return 
                !audioSourceBgm.isPlaying &&
                audioSourceBgm.time >= audioSourceBgm.clip.length;
        }
    }
}