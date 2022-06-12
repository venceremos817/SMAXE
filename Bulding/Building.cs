using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [ReadOnly]
    public int level = 0;
    [ReadOnly]
    public int buttonID = -1;   // 一意のボタンID
    [SerializeField]
    private GameObject[] buildingParts;
    [SerializeField, Tooltip("選択時に表示される説明")]
    public string explanation = "explanation";


    private void Start()
    {
        buttonID = GetComponent<Button>().GetInstanceID();

        // パーツをすべてオフ
        foreach (var part in buildingParts)
        {
            part.SetActive(false);
        }

        // レベル分オン
        for (int i = 0; i < level; ++i)
        {
            buildingParts[i].SetActive(true);
        }
    }

    /// <summary>
    /// ボタンクリックイベント
    /// </summary>
    public void Select()
    {
        BuildingManager.Instance.Select(this);
    }

    /// <summary>
    /// 建築物を進化させる
    /// </summary>
    /// <returns></returns>
    public bool Evolution()
    {
        if (level < buildingParts.Length)
        {
            buildingParts[level].SetActive(true);
            ++level;
            return true;
        }

        return false;
    }
}
