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
    public GameObject[] m_playerPrefabs;
    public GameObject[] m_enemyPrefabs;

    //MapScene移行用Player情報
    public Vector3 m_playerMapPosition = Vector3.zero;
    public Quaternion m_playerMapRotation = Quaternion.identity;
    
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

        m_playerPrefabs = playerPrefabs;
        m_enemyPrefabs = enemyPrefabs;

        m_playerMapPosition = playerMapPos;
        m_playerMapRotation = playerMapRotate;

        SceneManager.LoadScene("Battle");
        //StartCoroutine(LoadScene("Battle"));
    }

    /// <summary>
    /// マップシーンを読み込む
    /// </summary>
    public void LoadMapScene()
    {
        SceneManager.LoadScene("Map");
        //StartCoroutine(LoadScene("Map"));
    }

    //IEnumerator LoadScene(string name)
    //{
    //    SceneManager.LoadScene(name);
    //    yield return null;
    //}
}
