using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultStamp : MonoBehaviour
{
    public enum RANK
    {
        DEFEAT,
        C,
        B,
        A,
        S,
        SS,

        MAX
    };

    [SerializeField]
    DeadEnemyCountData deadEnemyCountData;
    [SerializeField]
    private SpriteRenderer stamp;
    [SerializeField, EnumIndex(typeof(RANK))]
    private SpriteRenderer[] seals;
    [SerializeField, EnumIndex(typeof(RANK))]
    private GameObject[] messages;
    [SerializeField, Min(0.0f)]
    private float alphaSpeed = 0.1f;
    //[ReadOnly]
    public RANK rank;


    private void Start()
    {
        // 初期化
        stamp.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        foreach (var seal in seals)
        {
            seal.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            seal.gameObject.SetActive(false);
        }
        foreach (var message in messages)
        {
            message.SetActive(false);
        }
        
        // ランク計算
        CalcRank();
    }

    /// <summary>
    /// ランク計算
    /// </summary>
    private void CalcRank()
    {
        var sceneMng = SceneMngr.Instance;
        int targetNumOfBeats = sceneMng.TargetNumberOfBeats;
        // 目標討伐数に達しなかった
        if (deadEnemyCountData.total < targetNumOfBeats)
        {
            rank = RANK.DEFEAT;
            messages[(int)rank].SetActive(true);
            return;
        }

        // 残HP%でランク計算
        float HPPercentage = sceneMng.PlayerHP / sceneMng.MaxPlayerHP;
        float numerator = 1.0f;
        float denominator = (float)RANK.MAX;
        for (int i = (int)RANK.DEFEAT; i < (int)RANK.MAX; ++i)
        {
            if (HPPercentage <= numerator / denominator)
            {
                rank = (RANK)i;
                messages[(int)rank].SetActive(true);
                break;
            }
            numerator += 1.0f;
        }
    }

    /// <summary>
    /// スタンプフェードイン
    /// </summary>
    public void StarteFadeInStamp()
    {
        stamp.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        StartCoroutine(FadeInStamp());
    }

    /// <summary>
    /// ランクシールフェードイン
    /// </summary>
    public void StartFadeInSeal()
    {
        StartCoroutine(FadeInSeal(rank));
    }

    /// <summary>
    /// スタンプフェードインコルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator FadeInStamp()
    {
        while (true)
        {
            stamp.color = Vector4.MoveTowards(stamp.color, Color.white, alphaSpeed);
            if (stamp.color.a >= 1.0f)
                break;
            yield return null;
        }
    }

    /// <summary>
    /// ランクシールフェードインコルーチン
    /// </summary>
    /// <param name="rank"></param>
    /// <returns></returns>
    private IEnumerator FadeInSeal(RANK rank)
    {
        var seal = seals[(int)rank];

        seal.gameObject.SetActive(true);
        while (true)
        {
            seal.color = Vector4.MoveTowards(seal.color, Color.white, alphaSpeed);
            if (seal.color.a >= 1.0f)
                break;
            yield return null;
        }
    }
}
