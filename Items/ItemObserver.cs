using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObserver : MyObserver
{
    public override void OnNotify(GameObject sourceObj, ObserverMessage message)
    {
        switch (message)
        {
            case ObserverMessage.GET_KEY:
                GetKey();
                break;
        }
    }


    private void GetKey()
    {

    }
}
