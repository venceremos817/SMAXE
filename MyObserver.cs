using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObserverMessage
{
    GET_KEY,
    PLAYER_SHOT_START,  // プレイヤータックル終了
    PLAYER_SHOT_END,  // プレイヤータックル終了

    CHANGED_VALUE,     // 値が変更された

    DEAD,           // 死亡

    CLEARDIRECTION_FADEOUT_END
}


/// <summary>
/// 監視クラスインターフェイス
/// 監視対象からメッセージを受け取りメッセージに応じて処理を行う
/// </summary>
public interface ObserverInterface
{
    void OnNotify(GameObject sourceObj, ObserverMessage message);
    void Observation(SubjectInterface subject);
}

/// <summary>
/// 監視クラス(MonoBehavior非継承)
/// </summary>
public abstract class MyObserver : ObserverInterface
{
    public abstract void OnNotify(GameObject sourceObj, ObserverMessage message);

    public void Observation(SubjectInterface subject)
    {
        subject.AddObserver(this);
    }
}


/// <summary>
/// 監視クラス(MonoBehavior継承)
/// </summary>
public abstract class MyMonoBehaviourObserver : MonoBehaviour, ObserverInterface
{
    public abstract void OnNotify(GameObject sourceObj, ObserverMessage message);
    public void Observation(SubjectInterface subject)
    {
        subject.AddObserver(this);
    }
}



/// <summary>
/// 監視対象クラスインターフェイス
/// 監視しているObserverにメッセージを送る
/// </summary>
public interface SubjectInterface
{
    void AddObserver(ObserverInterface observer);
    void RemoveObserver(ObserverInterface observer);
    void Notify(GameObject sorceObj, ObserverMessage message);
}


/// <summary>
/// 監視対象クラス(MonoBehavior非継承)
/// </summary>
public class MySubject : SubjectInterface
{
    [ReadOnly, SerializeField]
    private List<ObserverInterface> observersList = new List<ObserverInterface>();


    public void AddObserver(ObserverInterface observer)
    {
        if (!observersList.Contains(observer))
            observersList.Add(observer);
    }

    public void RemoveObserver(ObserverInterface observer)
    {
        observersList.Remove(observer);
    }


    public void Notify(GameObject sorceObj, ObserverMessage message)
    {
        bool nullCheck = true;
        while (nullCheck)
        {
            for (int i = 0; i < observersList.Count; ++i)
            {
                if (observersList[i].ToString() == "null")
                {
                    observersList.RemoveAt(i);
                    break;
                }
                nullCheck = false;
            }
        }

        foreach (var observer in observersList)
        {
            observer.OnNotify(sorceObj, message);
        }

    }
}

/// <summary>
/// 監視対象クラス(MonoBehavior継承)
/// 監視しているObserverにメッセージを送る
/// </summary>
public class MyMonoBehaviourSubject : MonoBehaviour, SubjectInterface
{
    [ReadOnly, SerializeField]
    private List<ObserverInterface> observers = null;


    public void AddObserver(ObserverInterface observer)
    {
        if (observers == null)
            observers = new List<ObserverInterface>();
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(ObserverInterface observer)
    {
        observers.Remove(observer);
    }


    public void Notify(GameObject sorceObj, ObserverMessage message)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(sorceObj, message);
        }
    }
}



/// <summary>
/// 監視者であり監視対象でもある
/// </summary>
public abstract class MyObserverSubject : ObserverInterface, SubjectInterface
{
    [ReadOnly, SerializeField]
    private List<ObserverInterface> observersList = new List<ObserverInterface>();


    public abstract void OnNotify(GameObject sourceObj, ObserverMessage message);

    public void Observation(SubjectInterface subject)
    {
        subject.AddObserver(this);
    }



    public void AddObserver(ObserverInterface observer)
    {
        if (!observersList.Contains(observer))
            observersList.Add(observer);
    }

    public void RemoveObserver(ObserverInterface observer)
    {
        observersList.Remove(observer);
    }


    public void Notify(GameObject sorceObj, ObserverMessage message)
    {
        foreach (var observer in observersList)
        {
            observer.OnNotify(sorceObj, message);
        }
    }
}


public abstract class MyMonobehaviorObserverSubject : MonoBehaviour, ObserverInterface, SubjectInterface
{
    [ReadOnly, SerializeField]
    private List<ObserverInterface> observers = null;


    public abstract void OnNotify(GameObject sourceObj, ObserverMessage message);
    public void Observation(SubjectInterface subject)
    {
        subject.AddObserver(this);
    }


    public void AddObserver(ObserverInterface observer)
    {
        if (observers == null)
            observers = new List<ObserverInterface>();
        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    public void RemoveObserver(ObserverInterface observer)
    {
        observers.Remove(observer);
    }


    public void Notify(GameObject sorceObj, ObserverMessage message)
    {
        foreach (var observer in observers)
        {
            observer.OnNotify(sorceObj, message);
        }
    }
}
