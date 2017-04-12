using UnityEngine;
using System.Collections;

public class BoderScript : MonoBehaviour {
	public SpriteRenderer spr;
	public Sprite oldSprite;
	void Start () {
		Vector4 newBorder = new Vector4 (4, 4, 4, 4);
		Rect rect = new Rect( 0,0, oldSprite.texture.width, oldSprite.texture.height);
		Sprite newSprite= Sprite.Create(oldSprite.texture, rect, new Vector2(0,0),  100, 4, SpriteMeshType.FullRect,newBorder );
		spr.sprite = newSprite;
	}
	void Update () {
	
	}
}
