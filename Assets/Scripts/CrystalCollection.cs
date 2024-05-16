using UnityEngine;
using UnityEngine.SceneManagement;

public class CrystalCollection : MonoBehaviour
{
    public GameObject crystalsFolder; 
    public GameObject fogFolder;      
    public GameObject portalObject;   
    public AudioClip healSound;       // Sound for collecting crystals, except the last one
    public AudioClip portalSound;     
    public string nextSceneName = "Village"; 

    private int crystalsCollected = 0;
    private int totalCrystals;
    private AudioSource audioSource;

    void Start()
    {
        totalCrystals = crystalsFolder.transform.childCount;
        audioSource = GetComponent<AudioSource>();

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
            // Call the scene loading function with a 1-second delay
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
        SceneManager.LoadScene(nextSceneName);
    }

    // Method that is called after the new scene is loaded
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the target scene is loaded
        if (scene.name == nextSceneName)
        {
            audioSource.PlayOneShot(portalSound);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
