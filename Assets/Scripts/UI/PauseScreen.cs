using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreen : UIScreen
{
    [SerializeField] Button _resumeButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _quitButton;

    protected override void OnEnable()
    {
        base.OnEnable();

        _resumeButton.onClick.AddListener(OnResume);
        _settingsButton.onClick.AddListener(OnSettings);
        _quitButton.onClick.AddListener(OnQuit);
    }
    protected override void OnDisable()
    {
        base.OnDisable();

        _resumeButton.onClick.RemoveListener(OnResume);
        _settingsButton.onClick.RemoveListener(OnSettings);
        _quitButton.onClick.RemoveListener(OnQuit);
    }

    void OnResume() => OnScreenGameEvent.Invoke();
    void OnSettings() { }
    void OnQuit() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    public override void Close()
    {
        
    }

    public override void Open()
    {

    }
}
