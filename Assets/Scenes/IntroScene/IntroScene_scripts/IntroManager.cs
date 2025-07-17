using UnityEngine;
using UnityEngine.Video; // Необходим для работы с Video Player
using TMPro; // Необходим для работы с TextMeshPro
using UnityEngine.SceneManagement; // Необходим для загрузки сцен

public class IntroManager : MonoBehaviour
{
    [Header("Video Settings")]
    [Tooltip("Перетащите сюда компонент VideoPlayer с GameObject, который проигрывает видео.")]
    public VideoPlayer videoPlayer; // Ссылка на компонент VideoPlayer
    [Tooltip("Перетащите сюда GameObject с Raw Image, который отображает видео.")]
    public GameObject videoScreen; // Ссылка на GameObject, где отображается видео (твой Raw Image)

    [Header("Text Settings")]
    [Tooltip("Перетащите сюда GameObject с TextMeshProUGUI для отображения размышлений.")]
    public TextMeshProUGUI reflectionText; // Ссылка на TextMeshProUGUI для размышлений
    [TextArea(5, 10)] // Позволяет вводить многострочный текст в Инспекторе
    [Tooltip("Массив текстовых сегментов. Каждый сегмент будет показан последовательно.")]
    public string[] reflectionSegments; // Массив строк для разных частей текста
    [Tooltip("Время в секундах, в течение которого отображается каждый текстовый сегмент.")]
    public float textDisplayDuration = 3f; // Время показа каждого сегмента текста (в секундах)

    private int currentSegmentIndex = 0; // Текущий индекс сегмента текста
    private bool videoStarted = false; // Флаг для отслеживания начала воспроизведения видео
    private bool introFinished = false; // Флаг, указывающий, что все интро завершено (видео + текст)

    // Корутина для управления сменой текста. Сохраняем ссылку, чтобы можно было остановить.
    private Coroutine textDisplayCoroutine;

    void Start()
    {
        // Проверка, что все необходимые ссылки установлены в Инспекторе
        if (videoPlayer == null || videoScreen == null || reflectionText == null)
        {
            Debug.LogError("IntroManager: Не все ссылки установлены в Инспекторе! Пожалуйста, перетащите нужные объекты (Video Player, Video Screen, Reflection Text).");
            enabled = false; // Отключаем скрипт, чтобы избежать ошибок
            return;
        }

        // Изначально скрываем видеоэкран и текстовое поле, пока видео не начнет готовиться
        videoScreen.SetActive(false);
        reflectionText.gameObject.SetActive(false);

        // Подписываемся на события VideoPlayer:
        // prepareCompleted: срабатывает, когда видео загружено и готово к воспроизведению.
        videoPlayer.prepareCompleted += OnVideoPrepared;
        // loopPointReached: срабатывает, когда видео достигает конца (или точки зацикливания).
        videoPlayer.loopPointReached += OnVideoEnd;

        // Запускаем асинхронную подготовку видео. 
        // Видео не начнет играть, пока не будет готово (событие OnVideoPrepared).
        videoPlayer.Prepare();
        Debug.Log("IntroManager: Видео готовится к воспроизведению...");
    }

    // Этот метод вызывается, когда VideoPlayer завершил подготовку видео.
    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("IntroManager: Видео готово. Запуск воспроизведения и отображение текста.");
        videoScreen.SetActive(true); // Показываем экран с видео
        videoPlayer.Play(); // Начинаем воспроизведение видео
        videoStarted = true; // Устанавливаем флаг, что видео запущено
        reflectionText.gameObject.SetActive(true); // Показываем текстовое поле

        // Запускаем корутину для автоматического отображения сегментов текста
        textDisplayCoroutine = StartCoroutine(DisplayTextSegmentsRoutine());
    }

    // Метод Update() теперь не используется для смены текста по клику,
    // так как текст меняется автоматически через корутину.
    // Если нужна возможность "пропустить" текущий текст кликом,
    // можно добавить логику здесь (остановить корутину и сразу вызвать DisplayNextTextSegment).
    void Update()
    {
        // Пример (опционально): пропуск текущего текста по клику
        // if (videoStarted && !introFinished && Input.GetMouseButtonDown(0))
        // {
        //     if (textDisplayCoroutine != null)
        //     {
        //         StopCoroutine(textDisplayCoroutine); // Останавливаем текущий таймер
        //     }
        //     // Запускаем следующий сегмент текста немедленно, затем возобновляем таймер
        //     DisplayNextTextSegmentInternal(); // Внутренний вызов без запуска новой корутины
        //     if (currentSegmentIndex < reflectionSegments.Length) { // Если есть еще текст
        //         textDisplayCoroutine = StartCoroutine(WaitForNextTextSegment());
        //     }
        // }
    }


    // Корутина для автоматического отображения сегментов текста с задержкой
    System.Collections.IEnumerator DisplayTextSegmentsRoutine()
    {
        while (currentSegmentIndex < reflectionSegments.Length)
        {
            // Устанавливаем текст из текущего сегмента
            reflectionText.text = reflectionSegments[currentSegmentIndex];
            Debug.Log($"IntroManager: Отображен сегмент текста {currentSegmentIndex}: '{reflectionSegments[currentSegmentIndex]}'");

            // Ждем указанное количество секунд перед показом следующего сегмента
            yield return new WaitForSeconds(textDisplayDuration);

            currentSegmentIndex++; // Переходим к следующему сегменту
        }

        // После того как все сегменты текста показаны
        Debug.Log("IntroManager: Все размышления главного героя показаны.");
        // Опционально: можно скрыть текстовое поле после показа всего текста
        // reflectionText.gameObject.SetActive(false);

        // Если видео уже закончилось (например, оно короткое или текст показывался долго),
        // то сразу переходим к игровой сцене. Иначе, ждем завершения видео через OnVideoEnd.
        if (!videoPlayer.isPlaying && videoPlayer.isPrepared)
        {
            TransitionToGameScene();
        }
        // Устанавливаем introFinished в true, чтобы предотвратить повторные действия
        introFinished = true;
    }


    // Этот метод вызывается, когда VideoPlayer завершил воспроизведение видео.
    void OnVideoEnd(VideoPlayer vp)
    {
        Debug.Log("IntroManager: Видео завершилось. Переход к игровой сцене.");
        introFinished = true; // Интро полностью завершено
        // Если корутина с текстом еще работает, останавливаем ее.
        if (textDisplayCoroutine != null)
        {
            StopCoroutine(textDisplayCoroutine);
        }
        TransitionToGameScene(); // Переходим к следующей сцене
    }

    // Метод для безопасного перехода к следующей сцене.
    void TransitionToGameScene()
    {
        // Получаем индекс текущей сцены (IntroScene)
        int currentSceneBuildIndex = SceneManager.GetActiveScene().buildIndex;
        // Загружаем следующую сцену по порядку в Build Settings
        int nextSceneBuildIndex = currentSceneBuildIndex + 1;

        Debug.Log($"IntroManager: Загрузка следующей сцены по индексу: {nextSceneBuildIndex}");

        // Очень важно: Отписаться от событий VideoPlayer перед загрузкой новой сцены,
        // чтобы избежать ошибок или утечек памяти.
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.loopPointReached -= OnVideoEnd;
        }

        SceneManager.LoadScene(nextSceneBuildIndex); // Загружаем игровую сцену
    }

    // Этот метод вызывается, когда GameObject уничтожается.
    // Важно остановить корутины и отписаться от событий.
    void OnDestroy()
    {
        if (textDisplayCoroutine != null)
        {
            StopCoroutine(textDisplayCoroutine);
        }
        if (videoPlayer != null)
        {
            videoPlayer.prepareCompleted -= OnVideoPrepared;
            videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}