using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneManagerUI : MonoBehaviour
{

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
