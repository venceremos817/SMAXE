using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMngr : SingletonMonoBehaviour<SoundMngr>
{
    public enum E_SE
    {
        PLAYER_SHOT,    // �v���C���[�V���b�g��
        PLAYER_CHARGE,  // �v���C���[�`���[�W��
        PLAYER_AXE,     // �v���C���[�̍U�����Փˎ�

        ENEMY_GOLEM,    // �ڐG���S�[������HP��0����Ȃ���
        ENEMY_EXPLOD,   // explod���j��
        ENEMY_TURRET,   // turret�U����

        MENU_DECISION,  // ���j���[���莞
        MENU_SELECT,    // ���j���[�I����

        GAME_OVER,      // �Q�[���I�[�o�[��

        POISON,         // ��

        CHECK,          // �`�F�b�N�}�[�N
        UNDERBAR,       // �I��������

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

    [SerializeField, Range(0.0f, 1.0f), Tooltip("�}�X�^�[�{�����[��")]
    private float _Master_Volume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("BGM�{�����[��")]
    private float _BGM_Volume = 1.0f;
    [SerializeField, Range(0.0f, 1.0f), Tooltip("SE�{�����[��")]
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

    [SerializeField, Min(1), Tooltip("�����ɖ炵����SE�̐���")]
    private int multiSE_NUM = 10;

    private List<AudioSource> SE_audioSources = null;
    private AudioSource BGM_audioSource = null;

    private Coroutine bgmFadeCorutine = null;
    private float tmpBGM_Vol = 0.0f;

    protected override void Awake()
    {
        if (!CheckInstance())
        {
            Destroy(gameObject);    // �V�[�����܂����̂ŏd�����Ă�����Q�[���I�u�W�F�N�g���̂��j��
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
    /// SE�Đ�
    /// </summary>
    /// <param name="e_SE">�Đ�������SE</param>
    /// <param name="loop">���[�v���邩</param>
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
    /// BGM�Đ�
    /// </summary>
    /// <param name="e_BGM">�Đ�������BGM</param>
    /// <param name="loop">���[�v���邩</param>
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
    /// BGM��~
    /// </summary>
    /// <param name="fade">�t�F�[�h�A�E�g���邩</param>
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
    /// BGM�t�F�[�h�C���R���[�`��
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
    /// BGM�t�F�[�h�A�E�g�R���[�`��
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
    /// ���������BGM���Đ�����Ă��邩�ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsPlayBGM()
    {
        return BGM_audioSource.isPlaying;
    }

    /// <summary>
    /// ���g�p��AudioSource���擾
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
    /// ���ʒ�����K�p
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