using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalCollection : MonoBehaviour
{
    public GameObject crystalsFolder; // Ссылка на папку с кристаллами
    public GameObject fogFolder;      // Ссылка на папку с туманом
    public GameObject portalObject;   // Ссылка на объект "Portal"
    public AudioClip healSound;       // Звук для сбора кристаллов, кроме последнего
    public AudioClip portalSound;     // Звук для сбора последнего кристалла
    public string nextSceneName = "Village"; // Название следующей сцены

    private int crystalsCollected = 0;
    private int totalCrystals;
    private AudioSource audioSource;

    void Start()
    {
        totalCrystals = crystalsFolder.transform.childCount;
        audioSource = GetComponent<AudioSource>();

        // Подпишемся на событие SceneManager.sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Crystal"))
        {
            CollectCrystal(other.gameObject);
        }
        else if (other.CompareTag("Portal") && crystalsCollected == totalCrystals)
        {
            // Вызывать функцию загрузки сцены с задержкой 1 секунда
            Invoke("LoadNextScene", 1f);
        }
    }

    void CollectCrystal(GameObject crystal)
    {
        crystalsCollected++;
        Destroy(crystal);

        if (crystalsCollected < totalCrystals)
        {
            audioSource.PlayOneShot(healSound);
        }
        else if (crystalsCollected == totalCrystals)
        {
            audioSource.PlayOneShot(portalSound);
            ActivatePortal();
        }
    }

    void ActivatePortal()
    {
        fogFolder.SetActive(false);
        if (portalObject != null)
        {
            portalObject.SetActive(true);
        }
    }

    void LoadNextScene()
    {
        // Загрузить следующую сцену
        SceneManager.LoadScene(nextSceneName);
    }

    // Метод, который вызывается после загрузки новой сцены
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Проверяем, что загружена целевая сцена
        if (scene.name == nextSceneName)
        {
            // Воспроизводим звук portalSound
            audioSource.PlayOneShot(portalSound);
        }
    }

    // Отписываемся от события при уничтожении объекта
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}