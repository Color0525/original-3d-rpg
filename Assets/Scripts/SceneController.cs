using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// シーンを管理
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// シーン唯一のSceneController
    /// </summary>
    public static SceneController m_Instance { get; private set; }

    //BattleScene移行用戦闘キャラ情報
    public GameObject[] m_PlayerPrefabs { get; private set; }
    public GameObject[] m_EnemyPrefabs { get; private set; }

    //MapScene移行用Player情報
    public Vector3 m_PlayerMapPosition { get; private set; } 
    public Quaternion m_PlayerMapRotation { get; private set; }

void Awake()
    {
        //シーンをまたぐSceneControllerを唯一にする
        if (m_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    /// <summary>
    /// マップシーンを読み込む
    /// </summary>
    public void LoadMapScene()
    {
        SceneManager.LoadScene("Map");
    }

    /// <summary>
    /// バトルシーンを読み込み、情報を引き継ぐ
    /// </summary>
    /// <param name="playerPrefabs"></param>
    /// <param name="enemyPrefabs"></param>
    /// <param name="playerMapPos"></param>
    /// <param name="playerMapRotate"></param>
    public void LoadBattleScene(GameObject[] playerPrefabs, GameObject[] enemyPrefabs, Vector3 playerMapPos, Quaternion playerMapRotate)
    {
        foreach (var player in FindObjectsOfType<MapPlayerController>())
        {
            player.StopControl();
        }
        foreach (var enemy in FindObjectsOfType<MapEnemyController>())
        {
            enemy.StopControl();
        }

        m_PlayerPrefabs = playerPrefabs;
        m_EnemyPrefabs = enemyPrefabs;

        m_PlayerMapPosition = playerMapPos;
        m_PlayerMapRotation = playerMapRotate;

        SceneManager.LoadScene("Battle");
    }
}
