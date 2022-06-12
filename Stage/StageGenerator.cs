using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : SingletonMonoBehaviour<StageGenerator>
{
    [System.Serializable]
    class NormalStage
    {
        [SerializeField, Tooltip("通常ステージアンカー")]
        public Transform ancher = null;
        [SerializeField, Tooltip("通常ステージチャンク")]
        public GameObject[] chunkKind;    // ステージパーツ
        [SerializeField, Tooltip("1パーツあたりの大きさ")]
        public Vector2 chunkSize = new Vector2(40.0f, 40.0f);    // パーツサイズ
        [SerializeField, Tooltip("ステージのパーツ数(grid * grid) 奇数しか受け付けんぞ！")]
        public byte grid = 3;
        [SerializeField, Tooltip("チャンクと一緒にスクロールするオブジェクトのレイヤー")]
        public LayerMask followLayer;
    };
    [System.Serializable]
    class BossStage
    {
        [SerializeField, Tooltip("ボスステージアンカー")]
        public Transform ancher = null;
        [SerializeField, Tooltip("ボスステージチャンク")]
        public GameObject chunk;
    }

    // インスペクター表示
    [SerializeField, Header("通常ステージ")]
    private NormalStage normalStage;
    [SerializeField, Header("ボスステージ")]
    private BossStage bossStage;
    [SerializeField, Tooltip("ボスステージに移行する際にオフにするオブジェクト")]
    private GameObject[] bossHideObjects;

    // インスペクター非表示
    [HideInInspector]
    public Vector3 centerChunkPos;   // 現在の中央のパーツの座標  
    [HideInInspector]
    public List<StageChunk> stageChunks = new List<StageChunk>();   // ステージチャンクスクリプト配列


    void Start()
    {
        normalStage.ancher.gameObject.SetActive(true);
        bossStage.ancher.gameObject.SetActive(false);
        // ステージを生成
        GenerateNormalStage();
        GenerateBossStage();
    }

    void Update()
    {
        UpdateNormalStage();
    }

    private void GenerateNormalStage()
    {
        if (normalStage.grid % 2 == 0)
            normalStage.grid += 1;
        int chunksNum = normalStage.chunkKind.Length;
        int cellID = 0;
        Vector3 offset = GameMngr.Instance.player.transform.position;

        for (int y = -normalStage.grid / 2; y < normalStage.grid / 2 + 1; ++y)
        {
            for (int x = -normalStage.grid / 2; x < normalStage.grid / 2 + 1; ++x)
            {
                int idx = Random.Range(0, chunksNum);
                Vector3 pos = new Vector3(x * normalStage.chunkSize.x, y * normalStage.chunkSize.y, 0.0f) + offset;
                var chunk = Instantiate(normalStage.chunkKind[idx], pos, Quaternion.identity, normalStage.ancher);
                var cellIDcs = chunk.AddComponent<StageChunk>();
                cellIDcs.cellID = cellID;
                stageChunks.Add(cellIDcs);
                if (cellID == normalStage.grid * normalStage.grid / 2)
                    centerChunkPos = chunk.transform.position;
                cellID++;
            }
        }
    }

    private void GenerateBossStage()
    {
        Instantiate(bossStage.chunk, GameMngr.Instance.player.transform.position, Quaternion.identity, bossStage.ancher);
    }

    public void StartBossStage()
    {
        normalStage.ancher.gameObject.SetActive(false);
        foreach (var obj in bossHideObjects)
            obj.SetActive(false);
        GameMngr.Instance.player.transform.position = Vector3.zero;
        bossStage.ancher.gameObject.SetActive(true);
    }


    /// <summary>
    /// ノーマルステージの更新
    /// プレイヤーが中央チャンクから離れたらスクロール
    /// </summary>
    private void UpdateNormalStage()
    {
        if (!normalStage.ancher.gameObject.activeSelf)
            return;

        var playerPos = GameMngr.Instance.player.transform.position;
        var diffX = playerPos.x - centerChunkPos.x;
        var diffY = playerPos.y - centerChunkPos.y;
        int powGrid = normalStage.grid * normalStage.grid;

        // 横
        if (diffX < -normalStage.chunkSize.x * 0.5f)
        {
            // 左
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_LEFT, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }
        else if (diffX > normalStage.chunkSize.x * 0.5f)
        {
            // 右
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_RIGHT, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }

        // 縦
        if (diffY < -normalStage.chunkSize.y * 0.5f)
        {
            // 下
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_DOWN, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }
        else if (diffY > normalStage.chunkSize.y * 0.5f)
        {
            // 上
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_UP, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }
    }
}
