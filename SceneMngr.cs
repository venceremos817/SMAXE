using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneMngr : SingletonMonoBehaviour<SceneMngr>
{
    [SerializeField]
    private SceneData sceneData;
    [SerializeField]
    private Fade fade;
    private SceneData.E_SceneKind currentScene = SceneData.E_SceneKind.Max;
    private Coroutine coroutine = null;
    private float playerHP;
    private float maxPlayerHP;
    private int targetNumberOfBeats;    // �ڕW������

    public float PlayerHP { get => playerHP; }
    public float MaxPlayerHP { get => maxPlayerHP; }
    public int TargetNumberOfBeats { get => targetNumberOfBeats; }

    protected override void Awake()
    {
        if (!CheckInstance())
        {
            Destroy(gameObject);    // �V�[�����܂����̂ŏd�����Ă�����Q�[���I�u�W�F�N�g���̂��j��
        }
        else
        {
            currentScene = SceneData.E_SceneKind.Max;
            var currentActiveSceneName = SceneManager.GetActiveScene().name;
            for (int i = 0; i < sceneData.scenes.Length; i++)
            {
                SceneObject scene = sceneData.scenes[i];
                if (scene.sceneName == currentActiveSceneName)
                {
                    currentScene = (SceneData.E_SceneKind)i;
                    break;
                }
            }

            DontDestroyOnLoad(this);
        }
    }

    /// <summary>
    /// �V�[���J��
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="interval">�t�F�[�h�ɂ����鎞��(�b)</param>
    public void ChangeScene(SceneData.E_SceneKind nextScene, float interval)
    {
        if (coroutine == null)
        {
            switch (currentScene)
            {
                case SceneData.E_SceneKind.Game:
                    var gameMng = GameMngr.Instance;
                    playerHP = gameMng.playerCommonCS.editor.Hp;
                    maxPlayerHP = gameMng.playerCommonCS.maxHP;
                    targetNumberOfBeats = gameMng.TargetNumberOfBeats;
                    break;
            }
            coroutine = StartCoroutine(LoadScene(nextScene, interval));
        }
    }
    

    private IEnumerator LoadScene(SceneData.E_SceneKind sceneKind, float interval)
    {
        // �C�x���g�V�X�e��������
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        eventSystem.enabled = false;
        Debug.Log("fadeOut start");
        // �t�F�[�h�A�E�g
        yield return fade.FadeOut(interval);
        Debug.Log("fadeOut end");

        // �񓯊����[�h
        AsyncOperation asyncLoad = null;
        asyncLoad = SceneManager.LoadSceneAsync(sceneData.scenes[(int)sceneKind].sceneName);
        Debug.Log("Loading...");
        // ���[�h����������܂ő҂�
        yield return asyncLoad;
        // ���[�h���������Ă��A�N�e�B�u�ɂ͂��Ȃ�
        asyncLoad.allowSceneActivation = false;
        Debug.Log("Load Fin");

        switch (sceneKind)
        {
            case SceneData.E_SceneKind.Title:
                SoundMngr.Instance.PlayBGM(SoundMngr.E_BGM.TITLE);
                break;
            case SceneData.E_SceneKind.Game:
                SoundMngr.Instance.PlayBGM(SoundMngr.E_BGM.GAME);
                break;
            case SceneData.E_SceneKind.Result:
                SoundMngr.Instance.PlayBGM(SoundMngr.E_BGM.SCORE);
                break;
        }
        // ���[�h�������������߃A�N�e�B�u�ɂ���
        asyncLoad.allowSceneActivation = true;
        coroutine = null;

        // �t�F�[�h�C��
        Debug.Log("fadeIn start");
        yield return fade.FadeIn(interval);
        Debug.Log("fadeIn end");
        Time.timeScale = 1.0f;
        // �C�x���g�V�X�e���L����
        eventSystem = UnityEngine.EventSystems.EventSystem.current;
        eventSystem.enabled = true;
        currentScene = sceneKind;
    }
}