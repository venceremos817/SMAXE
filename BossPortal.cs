using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPortal : MonoBehaviour
{
    private LayerMask playerLayer;

    private void Start()
    {
        playerLayer = GameMngr.Instance.player.layer;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer)
        {
            StartBossStage();
            gameObject.SetActive(false);
        }
    }


    void StartBossStage()
    {
        var normalStage = GameMngr.Instance.stageManager;
        normalStage.StartBossStage();
    }
}
