using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class UI : MonoBehaviour
{
    public TMP_Text sensivityText;
    public Slider sensivitySlider;
    public GameObject settings;
    private bool isActive = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            settings.SetActive(!isActive);
            if (isActive == false)
            { 
                Time.timeScale = 0;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                isActive = true;
            }
            else
            { 
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isActive = false;
            }

        }    
    }
    public void SensChange()
    {
        GameObject.Find("Player").transform.GetChild(0).GetComponent<Look>().mouseSensivity = sensivitySlider.value * 100;
        sensivityText.text = (sensivitySlider.value * 100).ToString();
    }
}
