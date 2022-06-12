using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAttackCollision : MyMonoBehaviourObserver
{
    [Header("吹っ飛ばし力")]
    public float force = 3.0f;
    [SerializeField, Range(0, 1), Header("後ろ方向への吹っ飛ばし比率")]
    private float shotAngle = 0.8f;
    [SerializeField]
    private float damage = 50.0f;
    private Rigidbody2D playerRb;
    private CircleCollider2D circleCol;
    private List<GameObject> hitedEnemy = new List<GameObject>();



    public override void OnNotify(GameObject sourceObj, ObserverMessage message)
    {
        switch (message)
        {
            case ObserverMessage.PLAYER_SHOT_START:
                if (GameMngr.Instance.IsPlayer(sourceObj))
                    StartShot();
                break;

            case ObserverMessage.PLAYER_SHOT_END:
                if (GameMngr.Instance.IsPlayer(sourceObj))
                    EndShot();
                break;
        }
    }

    private void Start()
    {
        GameMngr.Instance.playerCommonCS.GetStateShot().AddObserver(this);
        playerRb = GameMngr.Instance.player.GetComponent<Rigidbody2D>();
        circleCol = GetComponent<CircleCollider2D>();
        circleCol.enabled = false;
    }

    private void StartShot()
    {
        circleCol.enabled = true;
        hitedEnemy.Clear();
        playerRb.gameObject.layer = LayerMask.NameToLayer("AttackPlayer");
    }

    private void EndShot()
    {
        circleCol.enabled = false;
        hitedEnemy.Clear();
        playerRb.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (GameMngr.Instance.IsEnemy(collision.gameObject))
            OnEnterEnemy(collision.gameObject, collision.ClosestPoint(transform.position));
    }

    private void OnEnterEnemy(GameObject enemy, Vector2 closestPoint)
    {
        CircleColProcess(enemy, closestPoint);
    }

    private void CircleColProcess(GameObject enemy, Vector2 closestPoint)
    {
        // すでに衝突済みなら何もしない
        if (hitedEnemy.Contains(enemy))
            return;
        hitedEnemy.Add(enemy);

        var enemyRb = enemy.GetComponent<Rigidbody2D>();

#if true
        var playerMove = playerRb.velocity;
        Vector2 diff = transform.position - enemy.transform.position;

        float t;
        float amX, amY;

        diff *= -1.0f;

        t = -(-diff.y * playerMove.x + diff.x * playerMove.y) / (diff.y * diff.y + diff.x * diff.x);
        amX = playerMove.x - diff.y * t;
        amY = playerMove.y + diff.x * t;

        float e = 1.0f;
        float avX, avY;
        float bvX, bvY;

        avX = (playerRb.mass * amX - amX * e * enemyRb.mass) / (playerRb.mass + enemyRb.mass);
        avY = (playerRb.mass * amY - amY * e * enemyRb.mass) / (playerRb.mass + enemyRb.mass);
        bvX = e * amX + avX;
        bvY = e * amY + avY;

        var impulseForce = new Vector2(bvX * 100.0f, bvY * 100.0f);
        Vector2 project = Vector3.Project(diff.normalized, playerRb.velocity.normalized);
        impulseForce = (diff.normalized * shotAngle - playerRb.velocity.normalized).normalized * playerRb.velocity.magnitude * force;
#else
        var shotDir = playerRb.velocity.normalized;
        var cross = shotDir.x * (enemy.transform.position.y - transform.position.y) - shotDir.y * (enemy.transform.position.x - transform.position.x);

        if (cross > 0)
            Debug.Log("ohira:ひだり");
        else
            Debug.Log("ohira:みぎ");

        var impulseVec = (Vector2)(enemy.transform.position - transform.position) - playerRb.velocity.normalized * a;
        enemyRb.velocity = Vector2.zero;
        enemyRb.AddForce(impulseVec * force, ForceMode2D.Impulse);
#endif
        // enemyをノックバックステートに変更
        var enemyCS = enemy.GetComponent<Enemy>();
        enemyCS.ChangeToKnockback(impulseForce);
        enemyCS.AddDamage(damage);
        SoundMngr.Instance.PlaySE(SoundMngr.E_SE.PLAYER_AXE);
        EffectsMngr.Instance.StartEffects(EffectsMngr.EffectKind.CONTACT, closestPoint);
    }


}
