using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultBoard : MonoBehaviour
{
    [SerializeField, Min(0.0f), Tooltip("’èˆÊ’u‚Ü‚Å‚ÌŽžŠÔ")]
    private float time = 3.0f;
    [SerializeField]
    private ScoreBarGraph scoreBarGraphCS = null;
    private float elapsedTime = 0.0f;
    private Vector3 startPos;

    private void Start()
    {
        elapsedTime = 0.0f;
        startPos = transform.position;
    }

    private void Update()
    {
        // ’èˆÊ’u‚Ü‚ÅˆÚ“®
        elapsedTime += Time.deltaTime;
        transform.position = Vector3.Lerp(startPos, new Vector3(transform.position.x, 0.0f, transform.position.z), elapsedTime / time);

        if (elapsedTime > time)
            scoreBarGraphCS.StartGrowGraphs();
    }
}
