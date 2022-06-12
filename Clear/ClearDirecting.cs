using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearDirecting : MonoBehaviour
{
    [SerializeField]
    private float fadeOutTime = 1.0f;
    [SerializeField]
    private GameObject[] hideObjects;

    public void StartClearDirecting()
    {
        if (hideObjects.Length > 0)
            foreach (var obj in hideObjects)
            {
                obj.SetActive(false);
            }
    }

    public void LoadResultScene()
    {
        SceneMngr.Instance.ChangeScene(SceneData.E_SceneKind.Result, fadeOutTime);
    }
}