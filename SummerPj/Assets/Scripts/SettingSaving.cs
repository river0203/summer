using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingSaving : MonoBehaviour
{
    LightingSettings _lightingSettings;
    Light _light;

    [SerializeField] float _soundVolume = 1;
    [SerializeField] float _FOV = 40;
    [SerializeField] float _Bright;
    [SerializeField] float _sensitivity_x = 1f;
    [SerializeField] float _sensitivity_y = 1f;
    [SerializeField] bool _Shadow = true;

    [SerializeField] GameObject _shadowCheckImage;

    [SerializeField] Text _soundVolumeText;
    [SerializeField] Text _FOVText;
    [SerializeField] Text _BrightText;
    [SerializeField] Text _sensitivity_X_Text;
    [SerializeField] Text _sensitivity_Y_Text;


    private void Awake()
    {
        _light = FindAnyObjectByType<Light>();
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
        _soundVolumeText.text = "º¼·ý : " + Mathf.Round(_soundVolume);
        _FOVText.text = "FOV : " + _FOV;
        _BrightText.text = "¹à±â : " + _Bright;
        _sensitivity_X_Text.text = "XÃà ¹Î°¨µµ : " + _sensitivity_x;
        _sensitivity_Y_Text.text = "YÃà ¹Î°¨µµ : " + _sensitivity_y;
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

        // ±×¸²ÀÚ Á¶Àý
        _light.shadows = _Shadow ? LightShadows.Soft : LightShadows.None;
    }

    public void ChangeSoundVolume(float value)
    {
        _soundVolumeText.text = "º¼·ý : " + Mathf.Round(value);
        _soundVolume = value / 100;
    }

    public void ChangeFOV(float value)
    {
        _FOV = value;

        if(_FOVText != null)
            _FOVText.text = "FOV : " + value;
    }

    public void ChangeBright(float value) 
    { 
        _Bright = value;

        if(_BrightText != null)
            _BrightText.text = "¹à±â : " + value;
    }

    public void ChangeXSensitivity(float value)
    {
        _sensitivity_x = value;

        if(_sensitivity_X_Text != null)
            _sensitivity_X_Text.text = "XÃà ¹Î°¨µµ : " + value;
    }

    public void ChangeYSensitivity(float value)
    {
        _sensitivity_y = value;

        if(_sensitivity_Y_Text != null)
            _sensitivity_Y_Text.text= "YÃà ¹Î°¨µµ : " + value;
    }

    public void ChangeShadow()
    {
        _Shadow = _Shadow ? false : true;
        _shadowCheckImage.SetActive(_Shadow);
    }
}
