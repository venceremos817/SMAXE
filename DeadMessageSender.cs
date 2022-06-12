using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadMessageSender : MySubject
{
    public void Init()
    {
        // Ž€–SƒJƒEƒ“ƒ^‚ÉŠÄŽ‹‚µ‚Ä‚à‚ç‚¤
        GameMngr.Instance.deadEnemyCounter.Observation(this);
    }

    public void SendDeadMessage(GameObject owner)
    {
        Notify(owner, ObserverMessage.DEAD);
    }
}
