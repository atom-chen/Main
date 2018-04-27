using UnityEngine;
using System.Collections;
public enum GameGrade{
    NORMAL,
    EASY,
    HARD,
}

public enum ControType
{
    KEYBOARD,
    MOUSE,
    TOUCH,
}

public class GameSetting : MonoBehaviour {
    public  double valume = 1;
    public  GameGrade grade = GameGrade.EASY;
    public  ControType contro = ControType.KEYBOARD;
    public  bool isFullscreen = true;
    public GameObject startMenu;
    public GameObject optionMenu;
    public void OnVolumeChange()
    {
        //print("OnVolumeChange");
        valume=UIProgressBar.current.value;
        print(valume);
    }
    public void OnGameGradeChange()
    {
        switch(UIPopupList.current.value)
        {
            case "Easy":
                grade=GameGrade.EASY;
                break;
            case "Normal":
                grade = GameGrade.NORMAL;
                break;
            case "Hard":
                grade = GameGrade.HARD;
                break;
            default:
                grade = GameGrade.EASY;
                break;
        }
    }
    public void OnControlTypeChange()
    {
        switch(UIPopupList.current.value)
        {
            case "Keyboard":
                contro = ControType.KEYBOARD;
                break;
            case "Mouse":
                contro = ControType.MOUSE;
                break;
            case "Touch":
                contro = ControType.TOUCH;
                break;
            default:
                contro = ControType.KEYBOARD;
                break;
        }
    }
    public void OnIsFullScreenChange()
    {
        isFullscreen=UIToggle.current.value;
    }
    public void OnOptionButtonClick()
	{
		startMenu.GetComponent<TweenPosition> ().PlayForward();
		optionMenu.GetComponent<TweenPosition> ().PlayForward();
    }
    public void OnCompleteSettingButtonClick()
    {
		startMenu.GetComponent<TweenPosition> ().PlayReverse ();
		optionMenu.GetComponent<TweenPosition> ().PlayReverse ();
    }
}
