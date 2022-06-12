using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreasureKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameMngr.Instance.IsPlayer(collision.gameObject))
        {
            gameObject.SetActive(false);
        }
    }
}
