using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FocusBar : MonoBehaviour
{
    public Slider slider;

	public void SetMaxFocus(float focus)
	{
		slider.maxValue = focus;
		slider.value = (int) focus;
	}

    public void SetFocus(float focus)
    {
    	slider.value = (int) focus;
    }
}
