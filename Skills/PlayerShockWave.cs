using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class PlayerShockWave : MyMonoBehaviourObserver
{
    [SerializeField]
    private float damage = 50.0f;
    [SerializeField]
    private CircleCollider2D waveCollider = null;
    [SerializeField]
    private float radius = 1.0f;
    [SerializeField, Tooltip("è’åÇîgÇ™ç≈ëÂÇ…Ç»ÇÈÇ‹Ç≈ÇÃéûä‘(s)")]
    private float timeToMax = 0.5f;
    [ReadOnly]
    public float debugTime = 0.0f;



    public override void OnNotify(GameObject sourceObj, ObserverMessage message)
    {
        switch (message)
        {
            case ObserverMessage.PLAYER_SHOT_END:
                if (sourceObj == GameMngr.Instance.IsPlayer(sourceObj))
                    StartWave();
                break;
        }
    }

    private void StartWave()
    {
        waveCollider.radius = 0.0f;
        waveCollider.enabled = true;
        debugTime = 0.0f;
    }

    private void EndWave()
    {
        waveCollider.radius = 0.0f;
        waveCollider.enabled = false;
    }

    private void Start()
    {
        GameMngr.Instance.playerCommonCS.GetStateShot().AddObserver(this);
        if (!waveCollider)
            waveCollider = GetComponent<CircleCollider2D>();
        waveCollider.radius = 0.0f;
    }

    private void FixedUpdate()
    {
        if (!waveCollider.enabled)
            return;

        if (waveCollider.radius > radius)
            EndWave();

        float deltaRadius = (radius / timeToMax) * Time.fixedDeltaTime;
        waveCollider.radius += deltaRadius;
        debugTime += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameMngr.Instance.IsEnemy(collision.gameObject))
            BlowOffEnemy(collision.gameObject);
    }

    private void BlowOffEnemy(GameObject enemy)
    {
        Vector2 blowDir = enemy.transform.position - gameObject.transform.position;
        var enemyRb = enemy.GetComponent<Rigidbody2D>();
        enemyRb.AddForce(blowDir.normalized * 100, ForceMode2D.Impulse);
        enemy.GetComponent<Enemy>().AddDamage(damage);
    }


}
