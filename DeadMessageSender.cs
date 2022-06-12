using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMessageSender : MySubject
{
    public void Init()
    {
        // ���S�J�E���^�ɊĎ����Ă��炤
        GameMngr.Instance.deadEnemyCounter.Observation(this);
    }

    public void SendDeadMessage(GameObject owner)
    {
        Notify(owner, ObserverMessage.DEAD);
    }
}