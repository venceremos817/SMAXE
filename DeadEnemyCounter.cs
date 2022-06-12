using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DeadEnemyCounter : MyObserverSubject
{
    public DeadEnemyCountData counterDataObj = null;
    private GameObject _owner = null;
    private GameObject lastBeatEnemy = null;    // ç≈å„Ç…ì|ÇµÇƒÇ¢ÇÈìG

    public GameObject owner
    {
        get { return _owner; }
    }
    public int[] deadCount
    {
        get { return counterDataObj.deadCount; }
    }
    public int deadTotal
    {
        get { return counterDataObj.total; }
    }
    public GameObject LastBeatEnemy
    {
        get { return lastBeatEnemy; }
    }


    public DeadEnemyCounter(GameObject owner, DeadEnemyCountData dataObj)
    {
        _owner = owner;
        counterDataObj = dataObj;
        counterDataObj.ResetData();
    }


    public override void OnNotify(GameObject sourceObj, ObserverMessage message)
    {
        if (GameMngr.Instance.IsEnemy(sourceObj) &&
            message == ObserverMessage.DEAD)
        {
            var enemyKind = (int)sourceObj.GetComponent<Enemy>().EnemyParameter.kind;
            ++counterDataObj.deadCount[enemyKind];
            ++counterDataObj.total;
            lastBeatEnemy = sourceObj;
            Notify(_owner, ObserverMessage.CHANGED_VALUE);
        }
    }
}
