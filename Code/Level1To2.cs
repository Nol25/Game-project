using UnityEngine;
using UnityEngine.SceneManagement;

public class Level1To2 : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
            SceneManager.LoadScene(2); 
        
    }
}

