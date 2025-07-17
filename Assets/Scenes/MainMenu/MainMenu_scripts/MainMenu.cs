using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Метод Start вызывается один раз при инициализации скрипта
    void Start()
    {
        // Здесь можно добавить любую логику, которая должна выполняться при запуске главного меню.
        // Например, Debug.Log("Главное меню загружено!");
    }

    // Метод Update вызывается каждый кадр
    void Update()
    {
        // Здесь можно добавить логику, которая должна постоянно проверяться в главном меню.
        // Например, Input.GetKeyDown(KeyCode.Escape) для выхода.
    }

    public void Playgame()
    {
        Debug.Log("Загрузка следующей сцены...");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ExitGame()
    {
        Debug.Log("Выход из игры...");
        Application.Quit();
    }
}