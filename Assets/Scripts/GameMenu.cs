using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public string SampleScene;

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision detected between " + gameObject.name + " and " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("StartMenu"))
        {
            SceneManager.LoadScene(SampleScene);
        }
    }
}
