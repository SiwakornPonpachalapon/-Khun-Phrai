using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public GameObject Paneel;
    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            bool isOpen = !Paneel.activeSelf;

            Paneel.SetActive(isOpen);

            if (isOpen)
            {
                // Pause
                Time.timeScale = 0f;

                // ปลดเมาส์
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                // Resume
                Time.timeScale = 1f;

                // ล็อกเมาส์กลับ
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }
    public void GoMainMenu()
    {
        //Debug.Log("กด Main Menu แล้ว!");

        Time.timeScale = 1f;
        PlayerManager.Instance.SaveGame();
        SceneManager.LoadScene("Menu");
    }
}
