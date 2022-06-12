using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreBarGraph : MonoBehaviour
{
    [System.Serializable]
    private struct Graph
    {
        public RectTransform barRectTransform;
        public Image barImage;
        public TextMeshProUGUI textMesh;
    }



    [SerializeField]
    private DeadEnemyCountData deadCountData;
    [SerializeField]
    private ResultStamp resultStampCS;
    [SerializeField, Min(0f)]
    private float maxHeight = 5.0f;
    [SerializeField, EnumIndex(typeof(EnemyParameter.EnemyKind))]
    private Graph[] graphs = new Graph[(int)EnemyParameter.EnemyKind.MAX];
    [SerializeField]
    private float time = 3.0f;
    [SerializeField]
    private Gradient colorGradient;
    private float correction = 1.0f;    // 補正
    private int maxBeat = 0;            // 最大討伐数
    private float elapsedTime = 0.0f;   // 経過時間
    private Coroutine currentCoroutine;

    public DeadEnemyCountData DeadCountData { get => deadCountData; }

    private void Start()
    {
        foreach (var graph in graphs)
        {
            var tempScale = graph.barRectTransform.localScale;
            tempScale.y = 0.0f;
            graph.barRectTransform.localScale = tempScale;
            graph.textMesh.SetText("0");
        }
    }

    [ContextMenu("グラフ成長開始")]
    public void StartGrowGraphs()
    {
        maxBeat = 0;
        foreach (var beat in deadCountData.deadCount)
        {
            if (maxBeat < beat)
                maxBeat = beat;
        }
        correction = 1.0f;
        if (maxHeight < maxBeat)
        {
            correction = maxHeight / maxBeat;
        }

        if (currentCoroutine == null)
        {
            currentCoroutine = StartCoroutine(GrowGraphs());
        }
    }

    [ContextMenu("グラフ成長終了")]
    private void EndGrowGraphs()
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }
    }

    [ContextMenu("リセット")]
    private void ResetGraphs()
    {
        foreach (var graph in graphs)
        {
            var tempScale = graph.barRectTransform.localScale;
            tempScale.y = 0.0f;
            graph.barRectTransform.localScale = tempScale;
        }
    }

    /// <summary>
    /// グラフ成長コルーチン
    /// </summary>
    /// <returns></returns>
    private IEnumerator GrowGraphs()
    {
        elapsedTime = 0.0f;
        while (true)
        {
            float rate = elapsedTime / time;
            if (rate > 1.0f)
                break;

            for (int i = 0; i < (int)EnemyParameter.EnemyKind.MAX; ++i)
            {
                // グラフ
                // 高さ
                var targetVal = deadCountData.deadCount[i] * correction;
                var currentScale = graphs[i].barRectTransform.localScale;
                //currentScale.y = Mathf.Lerp(currentScale.y, targetVal, rate);
                currentScale.y = Mathf.Lerp(0.0f, targetVal, rate);
                graphs[i].barRectTransform.localScale = currentScale;
                // 色
                float colorRate = currentScale.y / maxHeight;
                graphs[i].barImage.color = colorGradient.Evaluate(colorRate);

                // 討伐数
                float currentBeat = Convert.ToInt32(graphs[i].textMesh.text);
                float targetBeat = deadCountData.deadCount[i];
                //currentBeat = Mathf.Lerp(currentBeat, targetBeat, rate);
                currentBeat = Mathf.Lerp(0.0f, targetBeat, rate);
                graphs[i].textMesh.SetText(Convert.ToString((int)currentBeat));
            }
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        for (int i = 0; i < (int)EnemyParameter.EnemyKind.MAX; ++i)
        {
            // グラフ
            var targetVal = deadCountData.deadCount[i] * correction;
            var scale = graphs[i].barRectTransform.localScale;
            scale.y = targetVal;
            graphs[i].barRectTransform.localScale = scale;

            // 討伐数
            graphs[i].textMesh.SetText(Convert.ToString(deadCountData.deadCount[i]));
        }

        resultStampCS.StarteFadeInStamp();
        resultStampCS.StartFadeInSeal();
    }
}
