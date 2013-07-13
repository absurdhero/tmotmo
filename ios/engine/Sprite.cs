using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

public class Sprite : MarshalByRefObject {
    public Texture2D[] textures;
    public Transform transform = new Transform();
    public Rectangle cropArea = Rectangle.Empty;

	public int height = 0;
	public int width = 0;
	
	public int texture_index = 0;
	private bool texture_dirty = true;

    public bool isVisible { get; set; }

	public Vector3 screenPosition {
		get {
            return Camera.main.WorldToScreenPoint(transform.Translation);
		}
		set {
            transform.Translation = Camera.main.ScreenToWorldPoint(value);
		}
	}

	public Vector3 worldPosition {
		get {
            return transform.Translation;
		}
		set {
            transform.Translation = value;
        }
	}

	public Sprite() {
        isVisible = true;
	}

	virtual public Quad createMesh()
	{
        var quad = new Quad(transform.Translation, transform.Forward, transform.Down, transform.Right, width, height);

        if (cropArea != Rectangle.Empty) {
            quad.cropTextureToRectangle(cropArea, textures[texture_index].Width, textures[texture_index].Height);
        }

        return quad;
	}

	void Update() {
		if (texture_dirty) {
//			imageMaterial.SetTexture(textures[texture_index]);
			texture_dirty = false;
		}
	}
	
	public void visible(bool enable) {
		isVisible = enable;
	}
	
	public bool LastTexture() {
		return texture_index == (textures.Length - 1);
	}
	
	public virtual void DrawNextFrame() {
		nextFrame();
		Animate();
	}
	
	public void nextFrame() {
		texture_index = (texture_index + 1) % textures.Length;
	}
	
	public void Animate() {
		texture_dirty = true;
	}
	
	public void setFrame(int index) {
		texture_index = index;
	}
	
	/// Center of sprite in World space
	public Vector2 Center() {
		var centerOfSprite = textures[0].Bounds.Center;
		var centerOnScreen = Camera.main.WorldToScreen2D(Vector2.Zero) + new Vector2(centerOfSprite.X, centerOfSprite.Y);
		return Camera.main.ScreenToWorld2D(centerOnScreen);
	}
	
	public int PixelWidth() {
		return width;
	}
	
	public int PixelHeight() {
		return height;
	}

	/// The screen rect based on texture dimensions
	public Rectangle ScreenRect() {
		return ScreenRect(Camera.main);
	}

	public Rectangle ScreenRect(Camera camera) {
		return new Rectangle ((int) screenPosition.X, (int) screenPosition.Y, width, height);
	}

	/// Takes scale and rotation into account
//	public Rectangle WorldScreenRect(Camera camera) {
//		Vector3 pos = camera.WorldToScreenPoint(transform.position);
//		Vector3 outterPos = transform.TransformPoint(transform.position + new Vector3(width, height, transform.position.z));
//		return new Rect(pos.x, pos.y, outterPos.x - pos.x, outterPos.y - pos.y);
//	}

//	public Vector2 ScreenCenter() {
//		Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
//		return new Vector2(pos.x + width / 2, pos.y + height / 2);
//	}

	public bool Contains(Camera camera, Vector2 position) {
		return ScreenRect(camera).Contains(new Point((int) position.X, (int) position.Y));
	}
	
	public bool Contains(Camera camera, Vector3 position) {
		return ScreenRect(camera).Contains(new Point((int) position.X, (int) position.X));
	}

	public void setScreenPosition(float x, float y) {
        screenPosition = new Vector3(x, y, screenPosition.Z);
	}
	
	public void setScreenPosition(int x, int y) {
		setScreenPosition((float) x, (float) y);
	}
	
	public void setScreenPosition(Vector3 position) {
		setScreenPosition(position.X, position.Y);
	}
	
	public Vector3 getScreenPosition() {
        return screenPosition;
	}
	
	/* In viewport space, 0 and 1 are the edges of the screen. */
	public void setCenterToViewportCoord(float x, float y) {
		var centeredPosition = snapToPixel(Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f)));
		worldPosition = new Vector3(centeredPosition.X, centeredPosition.Y, worldPosition.Z) - new Vector3(Center(), 0);
	}
	
	public void setWorldPosition(float x, float y, float z) {
		setWorldPosition(new Vector3(x, y, z));
	}

	public void setWorldPosition(Vector3 pos) {
		worldPosition = snapToPixel(pos);
	}
	
	public void setDepth(float z) {
        var position = worldPosition;
        position.Z = z;
        worldPosition = position;
	}
	
//	public void move(Vector3 pixels) {
//		setScreenPosition(getScreenPosition() + pixels);
//	}
//
//	public void move(float x, float y) {
//		move(new Vector3(x, y));
//	}
//
//	public void smoothMove(Vector3 pixels) {
//		move(pixels * Time.deltaTime);
//	}
//
//	public void smoothMove(float x, float y) {
//		move(new Vector3(x * Time.deltaTime, y * Time.deltaTime));
//	}
//
//	public GameObject createPivotOnTopLeftCorner() {
//		var parent = new GameObject("Parent of " + gameObject.name);
//		copyTransformTo(parent);
//
//		// translate the parent 2 times the sprite height.
//		// implicitly translate the sprite in the opposite direction by the same amount.
//		parent.transform.Translate(0f, worldHeight, 0f);
//		gameObject.transform.parent = parent.transform;
//		
//		return parent;		
//	}
//
    public Transform createPivotOnCenter() {
        return transform.createPivotAtOffset(worldWidth / 2f, worldHeight / 2f);
	}
//	
//	public GameObject createPivotOnBottomRightCorner() {
//		var parent = new GameObject("Parent of " + gameObject.name);
//		copyTransformTo(parent);
//
//		// translate the parent 2 times the sprite width.
//		// implicitly translate the sprite in the opposite direction by the same amount.
//		parent.transform.Translate(worldWidth, 0f, 0f);
//		gameObject.transform.parent = parent.transform;
//
//		return parent;
//	}
//
//	public GameObject createPivotOnBottomLeftCorner() {
//		var parent = new GameObject("Parent of " + gameObject.name);
//		copyTransformTo(parent);
//		gameObject.transform.parent = parent.transform;
//		
//		return parent;
//	}
//	
	private float worldHeight {
        get { return (float) height / Camera.main.pixelHeight * Camera.main.orthographicSize * 2.0f; }
	}
	
	private float worldWidth {
        get { return (float) width / Camera.main.pixelWidth * Camera.main.orthographicSize * 2.0f; }
	}

	private Vector3 snapToPixel(Vector3 pos) {
		Vector3 newpos;
		float pixelRatio = (Camera.main.orthographicSize * 2) / Camera.main.pixelHeight;
		
		newpos.X = (float) Math.Round(pos.X / pixelRatio) * pixelRatio;
		newpos.Y = (float) Math.Round(pos.Y / pixelRatio) * pixelRatio;
		newpos.Z = pos.Z;
		return newpos;
	}

}
