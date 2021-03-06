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
    /// ?????{?^?????I?????ɌĂяo??
    /// </summary>
    /// <param name="building"></param>
    public void Select(Building building)
    {
        if (selectingBuildingID == building.buttonID)
        {
            // ?I???ς?
            building.Evolution();
        }
        else
        {
            // ???I??
            selectingBuildingID = building.buttonID;
            textMesh.SetText(building.explanation);
        }
    }
}
