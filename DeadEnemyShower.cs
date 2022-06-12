using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DeadEnemyCounterを監視し、変更があればテキストを変更する
/// </summary>
public class DeadEnemyShower : MyMonoBehaviourObserver
{
    [SerializeField]
    private Text text = null;
    [SerializeField]
    private EnemyParameter.EnemyKind kind = EnemyParameter.EnemyKind.Normal;

    private void Start()
    {
        Observation(GameMngr.Instance.deadEnemyCounter);
    }


    public override void OnNotify(GameObject sourceObj, ObserverMessage message)
    {
        var deadEnemyCounter = GameMngr.Instance.deadEnemyCounter;
        if (sourceObj == deadEnemyCounter.owner &&
            message == ObserverMessage.CHANGED_VALUE)
        {
            text.text = "" + deadEnemyCounter.deadCount[(int)kind];
        }
    }
}
