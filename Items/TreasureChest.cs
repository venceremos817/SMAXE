using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TreasureChest : MonoBehaviour
{
    [SerializeField]
    private TreasureChestParameter param;
    [SerializeField, ReadOnly]
    private float respownTime = 0.0f;
    private SpriteRenderer sprRenderer;

    private void Start()
    {
        respownTime = param.respownTime;
        sprRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (respownTime < 0.0f)
        {
            Respown();
        }
        else
            respownTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameMngr.Instance.IsPlayer(collision.gameObject))
            OnHitPlayer();
    }


    private void OnHitPlayer()
    {
        sprRenderer.color = Color.gray;
        GetComponent<CircleCollider2D>().enabled = false;
        respownTime = param.respownTime;
        GameMngr.Instance.buildSelect.Selecte();
    }

    private void Respown()
    {
        sprRenderer.color = Color.white;
        GetComponent<CircleCollider2D>().enabled = true;
    }
}
