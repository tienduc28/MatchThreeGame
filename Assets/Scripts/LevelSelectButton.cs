using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelSelectButton : MonoBehaviour
{
    public string levelToLoad;

    public GameObject star1, star2, star3;

    void Start()
    {
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);

        int starsEarned = PlayerPrefs.GetInt(levelToLoad + "Stars", 0);
        switch (starsEarned)
        {
            case 3:
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
                break;
            case 2:
                star1.SetActive(true);
                star2.SetActive(true);
                break;
            case 1:
                star1.SetActive(true);
                break;
            default:
                break;
        }
    }
    public void LoadLevel()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
