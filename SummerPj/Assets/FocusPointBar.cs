using UnityEngine;
using UnityEngine.UI;

public class FocusPointBar : MonoBehaviour
{
    public Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void Init(float maxFocusPoint)
    {
        _slider.maxValue = maxFocusPoint;
        _slider.value = maxFocusPoint;
    }

    public void SetCurrentFocusPoints(float currentFocusPoints)
    {
        _slider.value = currentFocusPoints;
    }
}
