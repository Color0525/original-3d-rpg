using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    [SerializeField] string m_mapSceneName = "Map";
    [SerializeField] string m_battleSceneName = "Battle";
    [SerializeField] GameObject m_fadePanelPrefab;
    [SerializeField] float m_fadeSpeed = 1.5f;
    [SerializeField] GameObject m_encountPrefab;

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
    public void CallLoadMapScene()
    {
        StartCoroutine(LoadMapScene());
    }

    IEnumerator LoadMapScene()
    {
        yield return StartCoroutine(FadeOut(m_fadeSpeed));
        SceneManager.LoadScene(m_mapSceneName);
    }

    IEnumerator FadeOut(float fadeSpeed)
    {
        GameObject go = Instantiate(m_fadePanelPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Image fadeImage = go.GetComponent<Image>();
        Color currentColor = fadeImage.color;
        while (fadeImage.color.a < 1f)
        {
            currentColor.a += fadeSpeed * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }
    }

    /// <summary>
    /// エンカウントで呼び出す。情報を引き継ぎ、バトルシーンを読み込む
    /// </summary>
    /// <param name="playerPrefabs"></param>
    /// <param name="enemyPrefabs"></param>
    /// <param name="playerMapPos"></param>
    /// <param name="playerMapRotate"></param>
    public void EncountLoadBattleScene(GameObject[] playerPrefabs, GameObject[] enemyPrefabs, Transform playerMapTransform)
    {
        //コントロール不可にする
        foreach (var player in FindObjectsOfType<MapPlayerController>())
        {
            player.StopControl();
        }
        foreach (var enemy in FindObjectsOfType<MapEnemyController>())
        {
            enemy.StopControl();
        }

        //情報の引き継ぎ
        m_PlayerPrefabs = playerPrefabs;
        m_EnemyPrefabs = enemyPrefabs;
        m_PlayerMapPosition = playerMapTransform.position;
        m_PlayerMapRotation = playerMapTransform.rotation;

        StartCoroutine(LoadBattleScene());
    }

    IEnumerator LoadBattleScene()
    {
        yield return StartCoroutine(EncountEffect());
        //yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(m_battleSceneName);
    }

    IEnumerator EncountEffect()
    {
        GameObject go = Instantiate(m_encountPrefab);
        Animator anim = go.GetComponent<Animator>();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public void DebugLoadScene(string sceneName)
    {
        if (sceneName == m_mapSceneName)
        {
            StartCoroutine(LoadMapScene());
        }
        else if (sceneName == m_battleSceneName)
        {
            StartCoroutine(LoadBattleScene());
        }
        else
        {
            Debug.Log($"Not {sceneName} Scene");
        }
    }
}
