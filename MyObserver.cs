using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ObserverMessage
{
    GET_KEY,
    PLAYER_SHOT_START,  // �v���C���[�^�b�N���I��
    PLAYER_SHOT_END,  // �v���C���[�^�b�N���I��

    CHANGED_VALUE,     // �l���ύX���ꂽ

    DEAD,           // ���S

    CLEARDIRECTION_FADEOUT_END
}


/// <summary>
/// �Ď��N���X�C���^�[�t�F�C�X
/// �Ď��Ώۂ��烁�b�Z�[�W���󂯎�胁�b�Z�[�W�ɉ����ď������s��
/// </summary>
public interface ObserverInterface
{
    void OnNotify(GameObject sourceObj, ObserverMessage message);
    void Observation(SubjectInterface subject);
}

/// <summary>
/// �Ď��N���X(MonoBehavior��p��)
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
/// �Ď��N���X(MonoBehavior�p��)
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
/// �Ď��ΏۃN���X�C���^�[�t�F�C�X
/// �Ď����Ă���Observer�Ƀ��b�Z�[�W�𑗂�
/// </summary>
public interface SubjectInterface
{
    void AddObserver(ObserverInterface observer);
    void RemoveObserver(ObserverInterface observer);
    void Notify(GameObject sorceObj, ObserverMessage message);
}


/// <summary>
/// �Ď��ΏۃN���X(MonoBehavior��p��)
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
/// �Ď��ΏۃN���X(MonoBehavior�p��)
/// �Ď����Ă���Observer�Ƀ��b�Z�[�W�𑗂�
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
/// �Ď��҂ł���Ď��Ώۂł�����
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