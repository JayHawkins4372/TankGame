using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private string gameSceneName;
    [SerializeField] private string menuSceneName;

    public void Play()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void Menu()
    {
        SceneManager.LoadScene(menuSceneName);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
