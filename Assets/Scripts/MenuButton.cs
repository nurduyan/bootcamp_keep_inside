using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IDataPersistence
{
	[SerializeField] MenuButtonController menuButtonController;
	[SerializeField] Animator animator;
	[SerializeField] AnimatorFunctions animatorFunctions;
	[SerializeField] int thisIndex;

	private int _levelToLoad;
	
	void Update()
    {
		if(menuButtonController.index == thisIndex)
		{
			animator.SetBool ("selected", true);
			if(Input.GetAxis ("Submit") == 1){
				animator.SetBool ("pressed", true);
			}else if (animator.GetBool ("pressed")){
				animator.SetBool ("pressed", false);
				animatorFunctions.disableOnce = true;
			}
		}else{
			animator.SetBool ("selected", false);
		}
		
		if (Input.GetKeyDown(KeyCode.Space))
		{
			SceneManager.LoadScene(_levelToLoad);
		}
	}
	public void LoadData(GameData data){
		_levelToLoad = data._lastLevelIndex;
	}
	public void SaveData(GameData data){
		//Nothing to save here
	}
}
