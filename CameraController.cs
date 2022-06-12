using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float delay = 10.0f;

    [SerializeField]
    private float magnitude;    // 揺れの大きさ
    [SerializeField]
    private float time;         // 揺れの継続時間

    [SerializeField]
    private GameObject clear;

    private GameObject player = null;
    private float orthSize;
    private Transform playerTransform;
    private Vector3 cameraPos;
    private DeadEnemyCounter counter;
    private int _targetNumOfBeats;
    private GameObject lastEnemy;
    private bool isEnd;
    private bool once;
    private Vector2 lastPos;

    private void Start()
    {
        player = GameMngr.Instance.player;
        orthSize = GetComponent<Camera>().orthographicSize;
        counter = GameMngr.Instance.deadEnemyCounter;
        _targetNumOfBeats = GameMngr.Instance.TargetNumberOfBeats;
        lastEnemy = null;
        isEnd = false;
        once = true;
    }

    private void FixedUpdate()
    {
        if (once)
        {
            if (counter.deadTotal >= _targetNumOfBeats)
            {
                lastEnemy = GameMngr.Instance.deadEnemyCounter.LastBeatEnemy;
                isEnd = true;
                once = false;
            }
        }


        if (!isEnd)
        {
            playerTransform = player.transform;
            cameraPos = transform.position;
            cameraPos.z = playerTransform.position.z;
            cameraPos = Vector3.Lerp(cameraPos, playerTransform.position, delay * Time.fixedUnscaledDeltaTime);
            cameraPos.z = transform.position.z;
            transform.position = cameraPos;
        }
        else
        {
            LastEnemy();
            Time.timeScale = 0.5f;
            //Enemy.EnemyTimeScale = 0.7f;
            orthSize -= Time.fixedUnscaledDeltaTime;
            if (orthSize < 4.0f)
                orthSize = 4.0f;
            GetComponent<Camera>().orthographicSize = orthSize;
        }


        #region カメラの引きパターン1
#if false

        if (player.GetComponent<Player>().state == Player.E_STATE.E_CHARGE)
        {
            if (orthSize < 15)
                orthSize += Time.deltaTime * 5;

            GetComponent<Camera>().orthographicSize = orthSize;
        }
        else if (player.GetComponent<Player>().state == Player.E_STATE.E_ATTACK)
        {
            if (orthSize > 8)
                orthSize -= Time.deltaTime * 8;

            GetComponent<Camera>().orthographicSize = orthSize;
        }
        else if(player.GetComponent<Player>().state == Player.E_STATE.E_MOVE)
        {
            if (orthSize > 8)
                orthSize -= Time.deltaTime * 8;

            GetComponent<Camera>().orthographicSize = orthSize;
        }

#endif
        #endregion
        #region カメラの引きパターン2
#if false
        if (player.GetComponent<Player>().state == Player.E_STATE.E_CHARGE)
        {
            playerTransform = player.transform;     // チャージ状態の時の情報
        }

        if (player.GetComponent<Player>().state == Player.E_STATE.E_ATTACK)
        {
            var atkPos = player.transform;

            var centerPosX = Mathf.Lerp(playerTransform.position.x, atkPos.position.x, 0.5f * Time.fixedDeltaTime);
            var centerPosY = Mathf.Lerp(playerTransform.position.y, atkPos.position.y, 0.5f * Time.fixedDeltaTime);
            cameraPos.x = centerPosX;
            cameraPos.y = centerPosY;
            transform.position = cameraPos;

            if (orthSize < 15)
                orthSize += Time.deltaTime * 10;

            GetComponent<Camera>().orthographicSize = orthSize;
        }
        else if (player.GetComponent<Player>().state == Player.E_STATE.E_MOVE)
        {
            if (orthSize > 8)
                orthSize -= Time.deltaTime * 10;

            GetComponent<Camera>().orthographicSize = orthSize;
        }
#endif
        #endregion
    }


    //private void LateUpdate()
    //{
    //    // プレイヤーに遅れて追従
    //    Transform playerTransform = player.transform;
    //    var cameraPos = transform.position;
    //    cameraPos.z = playerTransform.position.z;
    //    cameraPos = Vector3.Lerp(cameraPos, playerTransform.position, delay * Time.fixedDeltaTime);
    //    cameraPos.z = transform.position.z;
    //    transform.position = cameraPos;
    //}

    private IEnumerator Shaking(float magnitude, float time)
    {
        var pos = transform.position;

        var left = time;

        while (left >= 0)
        {
            var x = pos.x + Random.Range(-1f, 1f) * magnitude;
            var y = pos.y + Random.Range(-1f, 1f) * magnitude;

            transform.position = new Vector3(x, y, pos.z);

            left -= Time.unscaledDeltaTime;

            yield return null;
        }

        transform.position = pos;
    }

    #region SendMessage

    /// <summary>
    /// プレイヤーがチャージ状態の時の処理
    /// </summary>
    public void PullCameraView()
    {
        playerTransform = player.transform;
    }

    /// <summary>
    /// プレイヤーがショット状態の時の処理
    /// </summary>
    public void ShotCameraView()
    {
        var atkPos = player.transform;

        var centerPosX = Mathf.Lerp(playerTransform.position.x, atkPos.position.x, 0.5f * Time.fixedDeltaTime);
        var centerPosY = Mathf.Lerp(playerTransform.position.y, atkPos.position.y, 0.5f * Time.fixedDeltaTime);
        cameraPos.x = centerPosX;
        cameraPos.y = centerPosY;
        transform.position = cameraPos;
    }

    public void ShakeCmaera()
    {
        StartCoroutine(Shaking(magnitude, time));
    }


    private void LastEnemy()
    {
        if (lastEnemy == null) { return; }

        if (lastEnemy.activeSelf)
        {
            cameraPos = transform.position;
            cameraPos.z = lastEnemy.transform.position.z;
            cameraPos = Vector3.Lerp(cameraPos, lastEnemy.transform.position, delay * Time.fixedUnscaledDeltaTime);
            cameraPos.z = transform.position.z;
            transform.position = cameraPos;
        }
        else
        {
            lastEnemy = null;
            clear.SetActive(true);
        }


    }

    #endregion
}
