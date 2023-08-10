using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingSaving : MonoBehaviour
{
    LightingSettings _lightingSettings;
    PlayerInputAction _inputActions;

    public float _soundVolume = 1;
    public float _FOV = 40;
    public float _Bright;
    public float _sensitivity_x = 1f;
    public float _sensitivity_y = 1f;

    public Text _soundVolumeText;
    public Text _FOVText;
    public Text _BrightText;
    public Text _sensitivity_X_Text;
    public Text _sensitivity_Y_Text;


    private void Awake()
    {
        _inputActions = new PlayerInputAction();
        _lightingSettings = new LightingSettings();

        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        _soundVolume = 1;
        _FOV = 40f;
        _Bright = 1;
        _sensitivity_x = 1f;
        _sensitivity_y = 1f;
    }

    private void OnSceneLoaded(Scene scene)
    {
        if (scene.name == "Game")
        {
            ApplySettings();   
        }
    }

    public void ApplySettings()
    {
        // »ç¿îµå Á¶Àý
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = _soundVolume;
        }

        // Ä«¸Þ¶ó FOV
        Camera.main.fieldOfView = _FOV;

        // ¹à±â Á¶Àý
        _lightingSettings.aoExponentDirect = _Bright;


        // ¹Î°¨µµ Á¶Àý
        InputHandler _inputHandler = FindObjectOfType<InputHandler>();
        _inputHandler._sensitivity_x = _sensitivity_x;
        _inputHandler._sensitivity_y = _sensitivity_y;
    }

    public void ChangeSoundVolume(float value)
    {
        _soundVolume = value;
        _soundVolumeText.text = "º¼·ý : " + Mathf.Round(value);
    }

    public void ChangeFOV(float value)
    {
        _FOV = value;
        _FOVText.text = "FOV : " + value;
    }

    public void ChangeBright(float value) 
    { 
        _Bright = value;
        _BrightText.text = "¹à±â : " + value;
    }

    public void ChangeXSensitivity(float value)
    {
        _sensitivity_x = value;
        _sensitivity_X_Text.text = "XÃà ¹Î°¨µµ : " + value;
    }

    public void ChangeYSensitivity(float value)
    {
        _sensitivity_y = value;
        _sensitivity_Y_Text.text= "YÃà ¹Î°¨µµ : " + value;
    }
}
