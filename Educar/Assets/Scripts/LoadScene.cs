using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_FadeCanvasGroup = null;
    [SerializeField] private float m_TransitionDuration = 1f;

    private bool IsFading = false;


    public event Action BeforeLoad, AfterLoad;

    public static SceneController Instance;

    private float DeltaTime { get { return Time.timeScale < 0.1f ? Time.unscaledDeltaTime : Time.deltaTime; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
            DestroyImmediate(gameObject);
    }

    private void Start()
    {
        m_FadeCanvasGroup.alpha = 1f;
        StartCoroutine(Fade(0));

    }

    public void LoadScene(string targetScene)
    {
        Debug.Log("Load Scene");
        if (!IsFading)
            StartCoroutine(LoadSceneAdditive(targetScene));

    }

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadSceneAdditive(string sceneName)
    {
        IsFading = true;

        yield return Fade(1f);

        BeforeLoad?.Invoke();

        SceneManager.LoadScene(sceneName);

        AfterLoad?.Invoke();


        yield return new WaitForSeconds(0.1f);


        yield return StartCoroutine(Fade(0f));

        IsFading = false;
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float alphaStep = Mathf.Abs(targetAlpha - m_FadeCanvasGroup.alpha) / m_TransitionDuration;
        m_FadeCanvasGroup.blocksRaycasts = true;

        while (!Mathf.Approximately(m_FadeCanvasGroup.alpha, targetAlpha))
        {
            m_FadeCanvasGroup.alpha = Mathf.MoveTowards(m_FadeCanvasGroup.alpha, targetAlpha,
                alphaStep * DeltaTime);

            yield return null;
        }

        m_FadeCanvasGroup.blocksRaycasts = false;
    }
}
