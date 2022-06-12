using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "ScriptableObject/SceneData")]
public class SceneData : ScriptableObject
{
    public enum E_SceneKind
    {
        Title = 0,
        Game,
        Result,

        Max
    }

    [SerializeField]
    private SceneObject[] _scenes = new SceneObject[(int)E_SceneKind.Max];

    public SceneObject[] scenes { get { return _scenes; } }
    public SceneObject gameScene { get { return _scenes[(int)E_SceneKind.Game]; } }
    public SceneObject resultScene { get { return _scenes[(int)E_SceneKind.Result]; } }
}
