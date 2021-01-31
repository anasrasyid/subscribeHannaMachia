using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

public class ScheneChanger : MonoBehaviour
{
    public void Online() {  
        SceneManager.LoadScene("Map Online");  
    }  
    public void Offline() {  
        SceneManager.LoadScene("Map Offline");  
    }  
    public void MainMenu() {  
        SceneManager.LoadScene("MainMenu");  
    }  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
