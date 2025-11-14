using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject victoryPanel;

    [Header("Gameplay to disable on win")]
    [SerializeField] private MonoBehaviour[] componentsToDisableOnWin;

    [Header("Scenes")]
    [SerializeField] private string menuSceneName = "Menu";
    [SerializeField] private string gameSceneName = "SampleScene";

    private bool _won;

    private void Awake()
    {
        if (victoryPanel) victoryPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Victory()
    {
        if (_won) return;
        _won = true;

        foreach (var comp in componentsToDisableOnWin)
            if (comp) comp.enabled = false;

        if (victoryPanel) victoryPanel.SetActive(true);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnClick_BackToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(menuSceneName);
    }

    public void OnClick_Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameSceneName);
    }
}
