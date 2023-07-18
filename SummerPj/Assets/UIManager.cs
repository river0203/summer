using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
<<<<<<< HEAD
    public PlayerInventory playerInventory;
    EquipmentWindowUI _equipmentWindowUI;

    [Header("UI Windows")]
    public GameObject hudWindow;
=======
>>>>>>> parent of 624510c (PT21 :: 인벤토리 닫기)
    public GameObject selectWindow;

    public void OpenSelectWindow()
    {
        selectWindow.SetActive(true);
    }

    public void CloseSelectWindow()
    {
        selectWindow.SetActive(false);
    }
}
