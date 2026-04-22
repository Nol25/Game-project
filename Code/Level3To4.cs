using UnityEngine;
using UnityEngine.SceneManagement;

public class Level3To4 : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(4);

    }
}
