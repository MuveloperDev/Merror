using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider Stamina = null;

    public void UpdateStamina(float changed)
    {
        Stamina.value = changed;
    }
}
