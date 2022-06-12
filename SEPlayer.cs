using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEPlayer : MonoBehaviour
{
    [SerializeField]
    SoundMngr.E_SE e_SE;

    public void PlaySE()
    {
        SoundMngr.Instance.PlaySE(e_SE);
    }
}
