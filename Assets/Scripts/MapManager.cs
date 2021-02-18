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
    [SerializeField] PlayableDirector m_openingCutScene;
    [SerializeField] PlayableDirector m_continueCutScene;
    [SerializeField] GameObject m_mapPlayer;
    [SerializeField] GameObject m_mainVirtualCamera;

    //public bool m_newGame { get; private set; } = false;

    void Start()
    {
        //マウスカーソル非表示
        if (m_hideSystemMouseCursor)
        {
            Cursor.visible = false;
        }

        SceneController.m_Instance.FadeIn();

        //状態に応じCutScene再生
        if (SceneController.m_Instance.m_NewGame)
        {
            SceneController.m_Instance.SetNewGameFalse();
            StartCoroutine(PlayOpeningCutScene());
        }
        else if (SceneController.m_Instance.m_GameOver)
        {
            StartCoroutine(PlayContinueCutScene());
        }
        else
        {
            ActivationPlayer();
        }
    }

    /// <summary>
    /// OPを再生し、ActivationPlayer()
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayOpeningCutScene()
    {
        m_openingCutScene.gameObject.SetActive(true);
        m_openingCutScene.Play();
        while (m_openingCutScene.state == PlayState.Playing)
        {
            yield return new WaitForEndOfFrame();
        }
        ActivationPlayer();
        m_openingCutScene.gameObject.SetActive(false);
    }

    /// <summary>
    /// CutSceneを再生し、ActivationPlayer()
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayContinueCutScene()
    {
        m_continueCutScene.gameObject.SetActive(true);
        m_continueCutScene.Play();
        while (m_continueCutScene.state == PlayState.Playing)
        {
            yield return new WaitForEndOfFrame();
        }
        ActivationPlayer();
        m_continueCutScene.gameObject.SetActive(false);
    }

    /// <summary>
    /// PlayerとメインバーチャルカメラをActiveにする
    /// </summary>
    void ActivationPlayer()
    {
        m_mapPlayer.SetActive(true);
        m_mainVirtualCamera.SetActive(true);
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
