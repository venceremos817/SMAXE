using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallSceneChanger : MonoBehaviour
{
    [SerializeField]
    private SceneData.E_SceneKind nextScene;
    [SerializeField]
    private float fadeOutTime = 1.0f;


    public void CallSceneChange()
    {
        SceneMngr.Instance.ChangeScene(nextScene, fadeOutTime);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR // エディター上での終了処理
        UnityEditor.EditorApplication.isPlaying = false;
#else // アプリでの終了処理
    Application.Quit();
#endif
    }
}
