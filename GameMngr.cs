using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMngr : SingletonMonoBehaviour<GameMngr>
{
    [SerializeField]
    private DeadEnemyCountData deadEnemyCountData;
    public byte FPS = 60;
    public GameObject player = null;
    public EnemyCreater enemyCreater = null;
    public PlayerCommon playerCommonCS = null;
    public StageGenerator stageManager = null;
    public BuildSelect buildSelect = null;
    public PoseSelect poseSelect = null;
    public LayerMask playerLayer;
    public LayerMask enemyLayer;
    private DeadEnemyCounter _deadEnemyCounter;
    //[SerializeField]
    //private GameObject gameoverDirectingObj;
    [SerializeField]
    private int targetNumberOfBeats = 300;
    private bool bCleared = false;

    public DeadEnemyCounter deadEnemyCounter
    {
        get { return _deadEnemyCounter; }
    }
    public int TargetNumberOfBeats
    {
        get { return targetNumberOfBeats; }
    }

    private void Start()
    {
        Application.targetFrameRate = FPS;   // FPSŒÅ’è       
        _deadEnemyCounter = new DeadEnemyCounter(gameObject, deadEnemyCountData);
        Time.timeScale = 1.0f;
        Enemy.EnemyTimeScale = 1.0f;
        bCleared = false;
    }

    private void OnValidate()
    {
        Application.targetFrameRate = FPS;
    }

    private void Update()
    {
        if (!bCleared)
            if (deadEnemyCounter.deadTotal >= targetNumberOfBeats)
            {
                //gameoverDirectingObj.SetActive(true);
                SoundMngr.Instance.PlayBGM(SoundMngr.E_BGM.CLEAR);
                bCleared = true;
            }
    }


    public bool IsPlayer(GameObject obj)
    {
        return ((1 << obj.gameObject.layer) & playerLayer.value) != 0;
    }


    public bool IsEnemy(GameObject obj)
    {
        return ((1 << obj.gameObject.layer) & enemyLayer.value) != 0;
    }
}
