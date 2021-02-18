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
    [SerializeField] GameObject m_mapPlayer;
    [SerializeField] GameObject m_mainVirtualCamera;

    bool m_newGame = true;

    void Start()
    {
        //マウスカーソル非表示
        if (m_hideSystemMouseCursor)
        {
            Cursor.visible = false;
        }

        SceneController.m_Instance.FadeIn();

        //NewGameならOP再生
        if (m_newGame)
        {
            m_newGame = false;
            StartCoroutine(PlayOpeningCutScene());
        }
        else
        {
            ActivationPlayer();
        }
    }

    IEnumerator PlayOpeningCutScene()
    {
        m_openingCutScene.gameObject.SetActive(true);
        m_openingCutScene.Play();
        while (m_openingCutScene.state == PlayState.Playing)
        {
            yield return new WaitForEndOfFrame();
        }
        m_openingCutScene.gameObject.SetActive(false);
        ActivationPlayer();

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
