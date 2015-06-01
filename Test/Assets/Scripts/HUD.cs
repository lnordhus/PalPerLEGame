using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public GUISkin resourceSkin, ordersSkin;
	private const int ordersBarWidth = 150, resourceBarHeight = 40;
	private Player player;

	void Start () {
		player = transform.root.GetComponent<Player>();
	}

	void OnGUI () {
		if (player && player.isHuman) 
		{
			DrawOrdersBar();
			DrawResourceBar();
		}
	}

	private void DrawOrdersBar() 
	{
		GUI.skin = ordersSkin;
		GUI.BeginGroup(new Rect(Screen.width - ordersBarWidth, resourceBarHeight, ordersBarWidth, Screen.height - resourceBarHeight));
		GUI.Box(new Rect(0, 0, ordersBarWidth, Screen.height - resourceBarHeight), "");
		GUI.EndGroup();
	}

	private void DrawResourceBar() 
	{
		GUI.skin = resourceSkin;
		GUI.BeginGroup(new Rect(0, 0, Screen.width, resourceBarHeight));
		GUI.Box(new Rect(0, 0, Screen.width, resourceBarHeight), "");
		GUI.EndGroup();
	}
}
