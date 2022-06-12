using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    [SerializeField]
    SoundMngr.E_BGM e_BGM;
    [SerializeField]
    bool loop = false;

    public void PlayBGM()
    {
        SoundMngr.Instance.PlayBGM(e_BGM, loop);
    }
}
