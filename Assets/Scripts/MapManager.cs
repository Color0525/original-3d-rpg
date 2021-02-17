using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

/// <summary>
/// MapSceneを管理
/// </summary>
public class MapManager : MonoBehaviour
{
    [SerializeField] bool m_hideSystemMouseCursor = false;
    [SerializeField] PlayableDirector m_opening;

    bool m_newGame = true;//あとでfalse

    void Start()
    {
        if (m_hideSystemMouseCursor)
        {
            Cursor.visible = false;
        }

        SceneController.m_Instance.FadeIn();

        if (m_newGame)
        {
            m_opening.Play();
            m_newGame = false;
        }
    }

    /// <summary>
    /// MapUnitをコントロール不可にする
    /// </summary>
    public void Freeze()
    {
        foreach (var player in FindObjectsOfType<MapPlayerController>())
        {
            player.StopControl();
        }
        foreach (var enemy in FindObjectsOfType<MapEnemyController>())
        {
            enemy.StopControl();
        }
    }
}
