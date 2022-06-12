using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDirecting : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime = 1.0f;
    [SerializeField]
    private GameObject[] hideObjects;


    public void StartGameoverDirecting()
    {
        if (hideObjects.Length > 0)
            foreach (var obj in hideObjects)
            {
                obj.SetActive(false);
            }
        SoundMngr.Instance.StopBGM(false);
        SoundMngr.Instance.PlaySE(SoundMngr.E_SE.GAME_OVER, false);
    }

    public void LoadResultScene()
    {
        SceneMngr.Instance.ChangeScene(SceneData.E_SceneKind.Result, fadeOutTime);
    }
}
