using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private static bool isPaused = false;

    [SerializeField] private GameObject pauseMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                isPaused = true;

                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                AudioManager.instance.SetPauseSoundState();
            }
            else
                ResumeGame();
        }
    }

    public void ResumeGame()
    {
        isPaused = false;

        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        AudioManager.instance.UnsetPauseSoundState();
    }

    public void ToMainMenu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
    }
}
