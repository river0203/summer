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
        // ���� ����
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = _soundVolume;
        }

        // ī�޶� FOV
        Camera.main.fieldOfView = _FOV;

        // ��� ����
        _lightingSettings.aoExponentDirect = _Bright;


        // �ΰ��� ����
        InputHandler _inputHandler = FindObjectOfType<InputHandler>();
        _inputHandler._sensitivity_x = _sensitivity_x;
        _inputHandler._sensitivity_y = _sensitivity_y;
    }

    public void ChangeSoundVolume(float value)
    {
        _soundVolume = value;
        _soundVolumeText.text = "���� : " + Mathf.Round(value);
    }

    public void ChangeFOV(float value)
    {
        _FOV = value;
        _FOVText.text = "FOV : " + value;
    }

    public void ChangeBright(float value) 
    { 
        _Bright = value;
        _BrightText.text = "��� : " + value;
    }

    public void ChangeXSensitivity(float value)
    {
        _sensitivity_x = value;
        _sensitivity_X_Text.text = "X�� �ΰ��� : " + value;
    }

    public void ChangeYSensitivity(float value)
    {
        _sensitivity_y = value;
        _sensitivity_Y_Text.text= "Y�� �ΰ��� : " + value;
    }
}
