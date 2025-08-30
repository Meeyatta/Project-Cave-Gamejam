using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class SettingsMenu : MonoBehaviour
{
    public int CurScene;
    public List<GameObject> Other;
    public GameObject Options;

    public void Play()
    {
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
    }
    public void Settings()
    {
        Debug.Log("123");

        foreach (var v in Other) { v.SetActive(false); }
        Options.SetActive(true);
    }
    public void ubSettings()
    {
        Debug.Log("123");
        foreach (var v in Other) { v.SetActive(true); }
        Options.SetActive(false);
    }
    public void Swap(InputAction.CallbackContext context)
    {
        if (CurScene > 0) Options.SetActive(!Options.activeSelf);
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
