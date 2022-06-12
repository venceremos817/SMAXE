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
    private int targetNumberOfBeats;    // 目標討伐数

    public float PlayerHP { get => playerHP; }
    public float MaxPlayerHP { get => maxPlayerHP; }
    public int TargetNumberOfBeats { get => targetNumberOfBeats; }

    protected override void Awake()
    {
        if (!CheckInstance())
        {
            Destroy(gameObject);    // シーンをまたぐので重複していたらゲームオブジェクト自体も破棄
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
    /// シーン遷移
    /// </summary>
    /// <param name="nextScene"></param>
    /// <param name="interval">フェードにかかる時間(秒)</param>
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
        // イベントシステム無効化
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        eventSystem.enabled = false;
        Debug.Log("fadeOut start");
        // フェードアウト
        yield return fade.FadeOut(interval);
        Debug.Log("fadeOut end");

        // 非同期ロード
        AsyncOperation asyncLoad = null;
        asyncLoad = SceneManager.LoadSceneAsync(sceneData.scenes[(int)sceneKind].sceneName);
        Debug.Log("Loading...");
        // ロードが完了するまで待つ
        yield return asyncLoad;
        // ロードが完了してもアクティブにはしない
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
        // ロードが完了したためアクティブにする
        asyncLoad.allowSceneActivation = true;
        coroutine = null;

        // フェードイン
        Debug.Log("fadeIn start");
        yield return fade.FadeIn(interval);
        Debug.Log("fadeIn end");
        Time.timeScale = 1.0f;
        // イベントシステム有効化
        eventSystem = UnityEngine.EventSystems.EventSystem.current;
        eventSystem.enabled = true;
        currentScene = sceneKind;
    }
}
