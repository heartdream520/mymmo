using Assets.Scripts.Managers;
using Services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISet : UIWindow {

	public void OnchicBackGame()
    {
        this.OnClick_Close();
    }
    public void OnchickBackCharacterSelect()
    {
        MapService.Instance.CurrentMapId = 0;
        MouseManager.Instance.ToShowCursor();
        UserService.Instance.SendCharacterLeave();
        MySceneManager.Instance.LoadScene("CharSelect");
    }
    public void OnchickExitGame()
    {
        
        UserService.Instance.SendCharacterLeave(true);
    }
    public void OnchickMusicSet()
    {
        this.OnClick_Close();
        UIManager.Instance.Show<UIMusicSet>();
    }
}
