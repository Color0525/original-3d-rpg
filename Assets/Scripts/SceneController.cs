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


    //public bool m_newGame { get; private set; } = true;//あとでfalse

    [SerializeField] string m_mapSceneName = "Map";
    [SerializeField] string m_battleSceneName = "Battle";
    [SerializeField] GameObject m_fadePanelPrefab;
    [SerializeField] float m_fadeSpeed = 1f;
    [SerializeField] GameObject m_encountEffectPrefab;
    [SerializeField] GameObject m_slideEffectPrefab;

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
        StartCoroutine(LoadSceneCoroutine(m_mapSceneName));
        //StartCoroutine(FadeIn(m_fadeSpeed));
    }

    IEnumerator FadeOut(float fadeSpeed = 1f)
    {
        GameObject go = Instantiate(m_fadePanelPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Image fadeImage = go.GetComponent<Image>();
        fadeImage.color = Color.clear;
        Color currentColor = fadeImage.color;
        while (fadeImage.color.a < 1f)
        {
            currentColor.a += 1 / fadeSpeed * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }
        Destroy(go, fadeSpeed);
    }
    public IEnumerator FadeIn(float fadeSpeed = 1f)
    {
        GameObject go = Instantiate(m_fadePanelPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Image fadeImage = go.GetComponent<Image>();
        fadeImage.color = Color.black;
        Color currentColor = fadeImage.color;
        while (fadeImage.color.a > 0f)
        {
            currentColor.a -= 1f / fadeSpeed * Time.deltaTime;
            fadeImage.color = currentColor;
            yield return null;
        }
        Destroy(go, fadeSpeed);
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
        StartCoroutine(LoadSceneCoroutine(m_battleSceneName));
        //StartCoroutine(FadeIn(m_fadeSpeed));
        //StartCoroutine(SlideEffect());
    }
    IEnumerator EncountEffect()
    {
        GameObject go = Instantiate(m_encountEffectPrefab);
        Animator anim = go.GetComponent<Animator>();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(go);
    }
    public IEnumerator SlideEffect()
    {
        GameObject go = Instantiate(m_slideEffectPrefab, GameObject.FindWithTag("MainCanvas").transform);
        Animator anim = go.GetComponent<Animator>();
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return new WaitForEndOfFrame();
        }
        Destroy(go);
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        yield return null;

        //if (sceneName == m_mapSceneName)
        //{
        //    StartCoroutine(LoadMapScene());
        //}
        //else if (sceneName == m_battleSceneName)
        //{
        //    StartCoroutine(LoadBattleScene());
        //}
        //else
        //{
        //    Debug.Log($"Not {sceneName} Scene");
        //}
    }
}
