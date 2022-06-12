using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Building : MonoBehaviour
{
    [ReadOnly]
    public int level = 0;
    [ReadOnly]
    public int buttonID = -1;   // ��ӂ̃{�^��ID
    [SerializeField]
    private GameObject[] buildingParts;
    [SerializeField, Tooltip("�I�����ɕ\����������")]
    public string explanation = "explanation";


    private void Start()
    {
        buttonID = GetComponent<Button>().GetInstanceID();

        // �p�[�c�����ׂăI�t
        foreach (var part in buildingParts)
        {
            part.SetActive(false);
        }

        // ���x�����I��
        for (int i = 0; i < level; ++i)
        {
            buildingParts[i].SetActive(true);
        }
    }

    /// <summary>
    /// �{�^���N���b�N�C�x���g
    /// </summary>
    public void Select()
    {
        BuildingManager.Instance.Select(this);
    }

    /// <summary>
    /// ���z����i��������
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