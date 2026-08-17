/*
   - KR.Sound - (Unity)
   ver.2026/08/17
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

        //BGMの開始位置をDSP時刻で管理.
        double bgmStartDspTime;
        bool   isBgmScheduled;

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
            if (!bgmClips.TryGetValue(name, out var bgm))
            {
                return;
            }

            //BGMを設定.
            audioSourceBgm.clip = bgm;
            audioSourceBgm.loop = isLoop;

            //現在のDSP時刻から少し先をBGM開始時刻にする.
            //少し先に予約することで、フレームタイミングに左右されにくくする.
            bgmStartDspTime = AudioSettings.dspTime + 0.1;

            //BGM開始済みフラグ.
            isBgmScheduled = true;

            //指定したDSP時刻にBGMを再生.
            audioSourceBgm.PlayScheduled(bgmStartDspTime);
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
            //BGMを停止.
            audioSourceBgm.Stop();

            //予約状態を解除.
            isBgmScheduled = false;
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
            //BGMが設定されていなければ終了.
            if (audioSourceBgm.clip == null)
            {
                return;
            }

            //指定時間を曲の長さの範囲に制限.
            float clampTime = Mathf.Clamp(
                time,
                0.0f,
                audioSourceBgm.clip.length
            );

            //DSP上の新しい開始時刻を計算.
            bgmStartDspTime = AudioSettings.dspTime - clampTime;

            //実際の再生位置も指定位置へ移動.
            audioSourceBgm.time = clampTime;

            //再生中なら、以降の時間基準をDSPに合わせる.
            if (audioSourceBgm.isPlaying)
            {
                isBgmScheduled = true;
            }
        }

        /// <summary>
        /// BGMの現在の再生時間を取得.
        /// </summary>
        public float GetTimeBGM()
        {
            //BGMが設定されていなければ0を返す.
            if (audioSourceBgm.clip == null)
            {
                return 0.0f;
            }
            //BGM未開始なら0秒.
            if (!isBgmScheduled)
            {
                return 0.0f;
            }

            //DSP時刻からBGM開始時刻を引いて再生時間を取得.
            double time = AudioSettings.dspTime - bgmStartDspTime;

            //曲の範囲に収める.
            return Mathf.Clamp(
                (float)time,
                0.0f,
                audioSourceBgm.clip.length
            );
        }

        /// <summary>
        /// BGMの最後まで再生されたか取得.
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

            //DSP基準で曲の長さまで到達したか確認.
            return
                isBgmScheduled &&
                AudioSettings.dspTime - bgmStartDspTime >= audioSourceBgm.clip.length;
        }
    }
}