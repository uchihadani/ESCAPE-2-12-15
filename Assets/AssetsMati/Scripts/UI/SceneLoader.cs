using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("Loading UI")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private Slider progressSlider;

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadAsync(sceneName));
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        if (loadingPanel) loadingPanel.SetActive(true);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            // op.progress va de 0 a 0.9, el 1.0 es cuando activás la escena
            float progress = Mathf.Clamp01(op.progress / 0.9f);
            if (progressSlider) progressSlider.value = progress;

            // Cuando esté listo (≥0.9), activamos
            if (op.progress >= 0.9f)
            {
                // Pequeño delay opcional para ver el 100%
                yield return new WaitForSeconds(0.1f);
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
