using System.Collections;
using UnityEngine;

/// <summary>
/// Attach this script to as many buttons as you want. Then in the On Click section of the editor 
/// (for each button you attach this to), click '+', add the button to itself as the game object, 
/// and then select "DoubleClickTest -> startClick()' as the function to call when the button is pressed.
/// </summary>

public class DoubleClickTest : MonoBehaviour
{
    private float doubleClickTimeLimit = 0.5f;
    bool clickedOnce = false;
    public string singleTapMove;
    public string doubleTapMove;
    float count = 0f;

    public void startClick()
    {
        StartCoroutine(ClickEvent());
    }

    public IEnumerator ClickEvent()
    {
        if (!clickedOnce && count < doubleClickTimeLimit)
        {
            clickedOnce = true;
        }
        else
        {
            clickedOnce = false;
            yield break;  //If the button is pressed twice, don't allow the second function call to fully execute.
        }
        yield return new WaitForEndOfFrame();

        while (count < doubleClickTimeLimit)
        {
            if (!clickedOnce)
            {
                DoubleClick();
                count = 0f;
                clickedOnce = false;
                yield break;
            }
            count += Time.deltaTime;// increment counter by change in time between frames
            yield return null; // wait for the next frame
        }
        SingleClick();
        count = 0f;
        clickedOnce = false;
    }
    private void SingleClick()
    {
        Debug.Log("Single Click");
    }

    private void DoubleClick()
    {
        Debug.Log("Double Click");
    }
}
