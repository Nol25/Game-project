using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2To3 : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(3);

    }
}
