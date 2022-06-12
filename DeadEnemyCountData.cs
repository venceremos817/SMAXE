using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/DeadEnemyCountData")]
public class DeadEnemyCountData : ScriptableObject
{
    [EnumIndex(typeof(EnemyParameter.EnemyKind))]
    public int[] deadCount = new int[(int)EnemyParameter.EnemyKind.MAX];
    public int total = 0;

    public void ResetData()
    {
        total = 0;
        for (int i = 0; i < (int)EnemyParameter.EnemyKind.MAX; ++i)
            deadCount[i] = 0;
    }

    [ContextMenu("Calc Total")]
    private void CalcTotal()
    {
        total = 0;
        foreach (var cnt in deadCount)
            total += cnt;
    }
}
