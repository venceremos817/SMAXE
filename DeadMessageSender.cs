using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMessageSender : MySubject
{
    public void Init()
    {
        // 死亡カウンタに監視してもらう
        GameMngr.Instance.deadEnemyCounter.Observation(this);
    }

    public void SendDeadMessage(GameObject owner)
    {
        Notify(owner, ObserverMessage.DEAD);
    }
}
