using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SceneLoader))]
public class MainMenuController : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button btnPlay;
    [SerializeField] private Button btnSettings;
    [SerializeField] private Button btnQuit;

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private CanvasGroup canvasGroup; // del Canvas para fade

    [Header("Config")]
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private float fadeDuration = 0.25f;

    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = GetComponent<SceneLoader>();
        if (settingsPanel) settingsPanel.SetActive(false);
        if (canvasGroup) canvasGroup.alpha = 0f;
    }

    private void Start()
    {
        if (canvasGroup) StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));

        if (btnPlay) btnPlay.onClick.AddListener(OnPlay);
        if (btnSettings) btnSettings.onClick.AddListener(OnOpenSettings);
        if (btnQuit) btnQuit.onClick.AddListener(OnQuit);
    }

    private void OnDestroy()
    {
        if (btnPlay) btnPlay.onClick.RemoveListener(OnPlay);
        if (btnSettings) btnSettings.onClick.RemoveListener(OnOpenSettings);
        if (btnQuit) btnQuit.onClick.RemoveListener(OnQuit);
    }

    private void OnPlay()
    {
        StartCoroutine(FadeOutAndLoad());
    }

    private void OnOpenSettings()
    {
        if (settingsPanel) settingsPanel.SetActive(true);
    }

    public void OnCloseSettings()
    {
        if (settingsPanel) settingsPanel.SetActive(false);
    }

    private void OnQuit()
    {
        _sceneLoader.QuitGame();
    }

    private IEnumerator FadeOutAndLoad()
    {
        if (canvasGroup) yield return FadeCanvas(1f, 0f, fadeDuration);
        _sceneLoader.LoadScene(gameSceneName);
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float t = 0f;
        canvasGroup.alpha = from;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}

