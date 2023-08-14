using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System;

public class SettingSaving : MonoBehaviour
{
    LightingSettings _lightingSettings;
    PlayerInputAction _inputActions;
    Light _light;

    [SerializeField] float _soundVolume = 1;
    [SerializeField] float _FOV = 40;
    [SerializeField] float _Bright;
    [SerializeField] float _sensitivity_x = 1f;
    [SerializeField] float _sensitivity_y = 1f;
    [SerializeField] bool _Shadow = true;

    [SerializeField] Text _soundVolumeText;
    [SerializeField] Text _FOVText;
    [SerializeField] Text _BrightText;
    [SerializeField] Text _sensitivity_X_Text;
    [SerializeField] Text _sensitivity_Y_Text;


    private void Awake()
    {
        _light = FindAnyObjectByType<Light>();
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
        
        ApplySettings();
        SettingOptions();
    }

    private void OnSceneLoaded(Scene scene)
    {
        SettingOptions();

        if (scene.name == "Game")
        {
            ApplySettings();
        }
    }

    void SettingOptions()
    {
        _soundVolumeText.text = "���� : " + Mathf.Round(_soundVolume);
        _FOVText.text = "FOV : " + _FOV;
        _BrightText.text = "��� : " + _Bright;
        _sensitivity_X_Text.text = "X�� �ΰ��� : " + _sensitivity_x;
        _sensitivity_Y_Text.text = "Y�� �ΰ��� : " + _sensitivity_y;
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

        // �׸��� ����
        _light.shadows = _Shadow ? LightShadows.Soft : LightShadows.None;
    }

    public void ChangeSoundVolume(float value)
    {
        _soundVolumeText.text = "���� : " + Mathf.Round(value);
        _soundVolume = value / 100;
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

    public void ChangeShadow()
    {
        if(_Shadow)
        {
            _Shadow = false;
        }
        else
        {
            _Shadow = true;
        }
        
        // ������ ����
    }
}
