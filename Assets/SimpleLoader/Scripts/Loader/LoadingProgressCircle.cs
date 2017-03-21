using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LoadingProgressCircle : MonoBehaviour {

    public Text ProgressText;
    public Image ProgressFillAmount;
    public Text PressAnyKeyText;


    public void SetProgress(float i)
    {
        GetComponent<Animator>().SetBool("isLoading", true);

        ProgressFillAmount.fillAmount = i;
        var progress = (int)(i * 100);
        ProgressText.text = progress + "%";

        if (progress > 95)
        { PressAnyKeyText.enabled = true;
            PressAnyKeyText.GetComponent<Animator>().SetBool("isLoaded", true);
        }
    }
}
