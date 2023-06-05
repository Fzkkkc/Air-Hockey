using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesMethods : MonoBehaviour
{
    private ScenesMethods _scenesMethods;
    
    public void Initialize(ScenesMethods controller)
    {
        _scenesMethods = controller;
    }
    
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }

    public void ExitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
