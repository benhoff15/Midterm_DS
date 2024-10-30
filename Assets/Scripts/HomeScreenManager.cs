using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScreenManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Duck Hunt Scene");
    }
}