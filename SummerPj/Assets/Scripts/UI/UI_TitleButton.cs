using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class UI_TitleButton : MonoBehaviour
{
    public void ClickAnim()
    {
        transform.GetComponentInChildren<VideoPlayer>().Play();
    }
}
