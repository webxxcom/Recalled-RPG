using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : UIScreen
{
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _quitButton;
    [SerializeField] VoidGameEvent OnPauseGameEvent;

    void OnEnable()
    {
        _resumeButton.onClick.AddListener(OnResume);
        _quitButton.onClick.AddListener(OnQuit);
    }
    void OnDisable()
    {
        _resumeButton.onClick.RemoveListener(OnResume);
        _quitButton.onClick.RemoveListener(OnQuit);
    }

    void OnResume() => OnPauseGameEvent.Invoke();
    void OnQuit() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
}
