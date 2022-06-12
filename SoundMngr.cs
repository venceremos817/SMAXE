using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMngr : SingletonMonoBehaviour<SoundMngr>
{
    public enum E_SE
    {
        PLAYER_SHOT,    // プレイヤーショット時
        PLAYER_CHARGE,  // プレイヤーチャージ時
        PLAYER_AXE,     // プレイヤーの攻撃が衝突時

        ENEMY_GOLEM,    // 接触時ゴーレムのHPが0じゃない時
        ENEMY_EXPLOD,   // explod爆破時
        ENEMY_TURRET,   // turret攻撃時

        MENU_DECISION,  // メニュー決定時
        MENU_SELECT,    // メニュー選択時

        GAME_OVER,      // ゲームオーバー時

        POISON,         // 毒

        CHECK,          // チェックマーク
        UNDERBAR,       // 選択時下線

        MAX
    }

    public enum E_BGM
    {
        TITLE,
        GAME,
        SCORE,

        CLEAR,

        MAX
    }

    [SerializeField, Range(0.0f, 1.0f), Tooltip("マスターボリューム")]
    private float _Master_Volume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("BGMボリューム")]
    private float _BGM_Volume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("SEボリューム")]
    private float _SE_Volume = 1.0f;

    public float MasterVolume
    {
        set
        {
            _Master_Volume = Mathf.Clamp01(value);
            BGM_audioSource.volume = _BGM_Volume * _Master_Volume;
            foreach (var se_source in SE_audioSources)
                se_source.volume = _SE_Volume * _Master_Volume;
        }
        get
        {
            return _Master_Volume;
        }
    }
    public float BGM_Volume
    {
        set
        {
            _BGM_Volume = Mathf.Clamp01(value);
            BGM_audioSource.volume = _BGM_Volume * _Master_Volume;
        }
        get
        {
            return _BGM_Volume;
        }
    }
    public float SE_Volume
    {
        set
        {
            _SE_Volume = Mathf.Clamp01(value);
            foreach (var se_source in SE_audioSources)
                se_source.volume = _SE_Volume * _Master_Volume;
        }
        get
        {
            return _SE_Volume;
        }
    }


    [SerializeField, EnumIndex(typeof(E_SE))]
    private AudioClip[] SE_audioClips = new AudioClip[(int)E_SE.MAX];
    [SerializeField, EnumIndex(typeof(E_BGM))]
    private AudioClip[] BGM_audioClips = new AudioClip[(int)E_BGM.MAX];

    [SerializeField, Min(0.0f)]
    private float BGM_FadeOutSpeed = 0.5f;
    [SerializeField, Min(0.0f)]
    private float BGM_FadeInSpeed = 0.5f;

    [SerializeField, Min(1), Tooltip("同時に鳴らしたいSEの数量")]
    private int multiSE_NUM = 10;

    private List<AudioSource> SE_audioSources = null;
    private AudioSource BGM_audioSource = null;

    private Coroutine bgmFadeCorutine = null;
    private float tmpBGM_Vol = 0.0f;

    protected override void Awake()
    {
        if (!CheckInstance())
        {
            Destroy(gameObject);    // シーンをまたぐので重複していたらゲームオブジェクト自体も破棄
        }
        else
        {
            if (SE_audioSources == null)
                SE_audioSources = new List<AudioSource>();
            for (int i = SE_audioSources.Count; i < multiSE_NUM; ++i)
            {
                var source = gameObject.AddComponent<AudioSource>();
                source.playOnAwake = false;
                SE_audioSources.Add(source);
            }

            if (BGM_audioSource == null)
            {
                BGM_audioSource = gameObject.AddComponent<AudioSource>();
                BGM_audioSource.playOnAwake = false;
            }

            ApplyVolume();
            PlayBGM(E_BGM.TITLE);

            DontDestroyOnLoad(this);
        }
    }

    private void OnValidate()
    {
        ApplyVolume();
    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="e_SE">再生したいSE</param>
    /// <param name="loop">ループするか</param>
    /// <returns></returns>
    public AudioSource PlaySE(E_SE e_SE, bool loop = false)
    {
        var source = GetUnusedSEAudioSource();
        if (source == null)
            return null;
        source.clip = SE_audioClips[(int)e_SE];
        source.loop = loop;
        source.Play();
        return source;
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="e_BGM">再生したいBGM</param>
    /// <param name="loop">ループするか</param>
    public void PlayBGM(E_BGM e_BGM, bool loop = true)
    {
        BGM_audioSource.Stop();
        BGM_audioSource.clip = BGM_audioClips[(int)e_BGM];
        BGM_audioSource.loop = loop;
        if (bgmFadeCorutine != null)
        {
            StopCoroutine(bgmFadeCorutine);
            BGM_Volume = tmpBGM_Vol;
        }
        bgmFadeCorutine = StartCoroutine(FadeInBGMCorutine());
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    /// <param name="fade">フェードアウトするか</param>
    public void StopBGM(bool fade = true)
    {
        if (fade)
        {
            if (bgmFadeCorutine != null)
            {
                StopCoroutine(bgmFadeCorutine);
                BGM_Volume = tmpBGM_Vol;
            }
            bgmFadeCorutine = StartCoroutine(FadeOutBGMCorutine());
        }
        else
        {
            BGM_audioSource.Stop();
        }
    }

    /// <summary>
    /// BGMフェードインコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInBGMCorutine()
    {
        tmpBGM_Vol = BGM_Volume;
        BGM_Volume = 0.0f;
        BGM_audioSource.Play();

        while (true)
        {
            BGM_Volume += BGM_FadeInSpeed * Time.unscaledDeltaTime;
            if (BGM_Volume >= tmpBGM_Vol)
            {
                BGM_Volume = tmpBGM_Vol;
                break;
            }
            yield return null;
        }
        bgmFadeCorutine = null;
    }

    /// <summary>
    /// BGMフェードアウトコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeOutBGMCorutine()
    {
        tmpBGM_Vol = BGM_Volume;

        while (true)
        {
            BGM_Volume -= BGM_FadeOutSpeed * Time.unscaledDeltaTime;
            if (BGM_Volume <= 0)
            {
                BGM_audioSource.Stop();
                BGM_Volume = tmpBGM_Vol;
                break;
            }
            yield return null;
        }
        bgmFadeCorutine = null;
    }

    /// <summary>
    /// 何かしらのBGMが再生されているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsPlayBGM()
    {
        return BGM_audioSource.isPlaying;
    }

    /// <summary>
    /// 未使用のAudioSourceを取得
    /// </summary>
    /// <returns></returns>
    private AudioSource GetUnusedSEAudioSource()
    {
        foreach (var source in SE_audioSources)
        {
            if (!source.isPlaying)
                return source;
        }
        return null;
    }

    /// <summary>
    /// 音量調整を適用
    /// </summary>
    private void ApplyVolume()
    {
        if (BGM_audioSource != null)
            BGM_audioSource.volume = _BGM_Volume * _Master_Volume;
        if (SE_audioSources != null)
            foreach (var se_source in SE_audioSources)
                se_source.volume = _SE_Volume * _Master_Volume;
    }
}
