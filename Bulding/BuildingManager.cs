using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuildingManager : SingletonMonoBehaviour<BuildingManager>
{
    [ReadOnly, SerializeField]
    private int selectingBuildingID = -1;
    [SerializeField]
    private TextMeshProUGUI textMesh;

    /// <summary>
    /// 建物ボタンを選択時に呼び出し
    /// </summary>
    /// <param name="building"></param>
    public void Select(Building building)
    {
        if (selectingBuildingID == building.buttonID)
        {
            // 選択済み
            building.Evolution();
        }
        else
        {
            // 未選択
            selectingBuildingID = building.buttonID;
            textMesh.SetText(building.explanation);
        }
    }
}
