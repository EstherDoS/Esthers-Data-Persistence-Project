using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

using UnityEngine.UI;
using TMPro;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public TextMeshProUGUI nameText;
    public void SetName(string name)
    {
        // add code here to handle when a color is selected
        MainManager.Instance.currentPlayer = name;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);  
    }
}
