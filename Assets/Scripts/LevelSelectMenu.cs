using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectMenu : MonoBehaviour
{
    public string mainMenu = "Main Menu";

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(mainMenu);
    }
}
