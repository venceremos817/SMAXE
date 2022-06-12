using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : SingletonMonoBehaviour<StageGenerator>
{
    [System.Serializable]
    class NormalStage
    {
        [SerializeField, Tooltip("�ʏ�X�e�[�W�A���J�[")]
        public Transform ancher = null;
        [SerializeField, Tooltip("�ʏ�X�e�[�W�`�����N")]
        public GameObject[] chunkKind;    // �X�e�[�W�p�[�c
        [SerializeField, Tooltip("1�p�[�c������̑傫��")]
        public Vector2 chunkSize = new Vector2(40.0f, 40.0f);    // �p�[�c�T�C�Y
        [SerializeField, Tooltip("�X�e�[�W�̃p�[�c��(grid * grid) ������󂯕t���񂼁I")]
        public byte grid = 3;
        [SerializeField, Tooltip("�`�����N�ƈꏏ�ɃX�N���[������I�u�W�F�N�g�̃��C���[")]
        public LayerMask followLayer;
    };
    [System.Serializable]
    class BossStage
    {
        [SerializeField, Tooltip("�{�X�X�e�[�W�A���J�[")]
        public Transform ancher = null;
        [SerializeField, Tooltip("�{�X�X�e�[�W�`�����N")]
        public GameObject chunk;
    }

    // �C���X�y�N�^�[�\��
    [SerializeField, Header("�ʏ�X�e�[�W")]
    private NormalStage normalStage;
    [SerializeField, Header("�{�X�X�e�[�W")]
    private BossStage bossStage;
    [SerializeField, Tooltip("�{�X�X�e�[�W�Ɉڍs����ۂɃI�t�ɂ���I�u�W�F�N�g")]
    private GameObject[] bossHideObjects;

    // �C���X�y�N�^�[��\��
    [HideInInspector]
    public Vector3 centerChunkPos;   // ���݂̒����̃p�[�c�̍��W  
    [HideInInspector]
    public List<StageChunk> stageChunks = new List<StageChunk>();   // �X�e�[�W�`�����N�X�N���v�g�z��


    void Start()
    {
        normalStage.ancher.gameObject.SetActive(true);
        bossStage.ancher.gameObject.SetActive(false);
        // �X�e�[�W�𐶐�
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
    /// �m�[�}���X�e�[�W�̍X�V
    /// �v���C���[�������`�����N���痣�ꂽ��X�N���[��
    /// </summary>
    private void UpdateNormalStage()
    {
        if (!normalStage.ancher.gameObject.activeSelf)
            return;

        var playerPos = GameMngr.Instance.player.transform.position;
        var diffX = playerPos.x - centerChunkPos.x;
        var diffY = playerPos.y - centerChunkPos.y;
        int powGrid = normalStage.grid * normalStage.grid;

        // ��
        if (diffX < -normalStage.chunkSize.x * 0.5f)
        {
            // ��
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_LEFT, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }
        else if (diffX > normalStage.chunkSize.x * 0.5f)
        {
            // �E
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_RIGHT, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }

        // �c
        if (diffY < -normalStage.chunkSize.y * 0.5f)
        {
            // ��
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_DOWN, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }
        else if (diffY > normalStage.chunkSize.y * 0.5f)
        {
            // ��
            for (int i = 0; i < powGrid; ++i)
                stageChunks[i].Shift(StageChunk.SHIFT.SHIFT_UP, normalStage.chunkSize, normalStage.grid, normalStage.followLayer);
        }
    }
}