﻿using UnityEngine;
using System.Collections;

[ExecuteInEditMode]public class UI_effect : MonoBehaviour {

	public int renderQueue = 3000;
	public bool runOnlyOnce = false;        
	void Start()    
	{  Update();   }       

	void Update()   
	{       
		if (renderer != null && renderer.sharedMaterial != null)        
		{           
			renderer.sharedMaterial.renderQueue = renderQueue;      
		}       
		if (runOnlyOnce && Application.isPlaying)      
		{           
			this.enabled = false;       
		}
        this.enabled = false;   //kino try
	}
}