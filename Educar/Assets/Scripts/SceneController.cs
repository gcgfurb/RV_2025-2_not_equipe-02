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

    // Armazena a última cena de gameplay antes do Game Over
    private string lastGameplayScene;

    private float DeltaTime { get { return Time.timeScale < 0.1f ? Time.unscaledDeltaTime : Time.deltaTime; } }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // Detecta mudanças de cena automaticamente
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
        else
        {
            lastGameplayScene = "MainMenu";
            DestroyImmediate(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
    }

    private void Start()
    {
        if (m_FadeCanvasGroup != null)
        {
            m_FadeCanvasGroup.alpha = 1f;
            StartCoroutine(Fade(0));
        }
        else
        {
            Debug.LogWarning("Fade CanvasGroup não atribuído no SceneController.");
        }
    }

    // Detecta mudança de cena e salva a cena anterior quando entrar no Game Over
    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        if (newScene.name == "GameOver" && previousScene.name != "GameOver")
        {
            lastGameplayScene = previousScene.name;
            Debug.Log($"[SceneController] Cena salva para reiniciar: {lastGameplayScene}");
        }
    }

    public void LoadScene(string targetScene)
    {
        Debug.Log("Load Scene: " + targetScene);

        if (targetScene == "GameOver")
        {
            lastGameplayScene = SceneManager.GetActiveScene().name;
        }

        if (!IsFading)
            StartCoroutine(LoadSceneAdditive(targetScene));
    }

    public void RestartScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    // Reinicia a última cena antes do Game Over
    public void RestartLevel()
    {
        if (!string.IsNullOrEmpty(lastGameplayScene))
        { 
            Debug.Log("Reiniciando cena: " + lastGameplayScene);
            SceneManager.LoadScene(lastGameplayScene);
        }
        else
        {
            Debug.LogWarning("Nenhuma cena registrada para reiniciar!");
        }
    }

    // NOVO — Carrega a próxima fase depois da FaseConcluida
    public void LoadNextLevel()
    {
        int currentIndex = PlayerPrefs.GetInt("LastPlayableSceneIndex", -1);

        if (currentIndex < 0)
        {
            Debug.LogError("Nenhuma fase registrada para continuar!");
            return;
        }

        int nextIndex = currentIndex + 1;
        Debug.Log("Carregando próxima fase: " + nextIndex);

        SceneManager.LoadScene(nextIndex);
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
        if (m_FadeCanvasGroup == null)
            yield break;

        float alphaStep = Mathf.Abs(targetAlpha - m_FadeCanvasGroup.alpha) / Mathf.Max(0.0001f, m_TransitionDuration);
        m_FadeCanvasGroup.blocksRaycasts = true;

        while (!Mathf.Approximately(m_FadeCanvasGroup.alpha, targetAlpha))
        {
            m_FadeCanvasGroup.alpha = Mathf.MoveTowards(m_FadeCanvasGroup.alpha, targetAlpha,
                alphaStep * DeltaTime);

            yield return null;
        }

        m_FadeCanvasGroup.blocksRaycasts = false;
    }

    public void QuitGame()
    {
        Debug.Log("Fechando o jogo...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
