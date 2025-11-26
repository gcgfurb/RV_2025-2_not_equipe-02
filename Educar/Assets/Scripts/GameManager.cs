using UnityEngine;

public class GameManager : MonoBehaviour
{

    public SceneController sceneController;
    public void Start()
    {
        sceneController = GameObject.FindAnyObjectByType<SceneController>();
    }

    public void LoadScene(string sceneName)
    {
        sceneController.LoadScene(sceneName);
    }

    public void RestarLevel()
    {
        sceneController.RestartLevel();
    }
}
