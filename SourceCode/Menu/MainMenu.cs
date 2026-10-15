using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Scene1");
    }
    public void NewGame()
    {
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Scene1");
    }
    public void Exit()
    {
        PlayerManager.Instance.SaveGame();
        Application.Quit();
    }
    public void GaMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void PlayerAgain()
    {
        if (QuestManager.Instance.currentQuest == 4)
        {
            SceneManager.LoadScene("Scene2");
        }
        else if (QuestManager.Instance.currentQuest == 5)
        {
            SceneManager.LoadScene("Scene3");
        }
    }
}
