using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using TexturedQuad;

public class Sprite {
	private GraphicsDeviceManager graphics;
	public Texture2D[] textures;
	Quad quad;

	public int height = 0;
	public int width = 0;
	
	private int texture_index = 0;
	private bool texture_dirty = true;

	private bool isVisible = true;

	private Vector2 _screenPosition;
	public Vector2 screenPosition {
		get {
			return _screenPosition;
		}
		set {
			_screenPosition = value;
			quad = new Quad(Camera.main.ScreenToWorldPoint(new Vector3(value, 0)), Vector3.Forward, Vector3.Down, width, height);
		}
	}
	public Vector2 worldPosition { 
		get {
			return Camera.main.ScreenToWorld2D(screenPosition);
		}
		set {
			screenPosition = Camera.main.WorldToScreen2D(value);
		}
	}


	/// Destroys the underlying GameObject
	public static void Destroy(Sprite sprite) {
	}

	static string[] pathsForClassName (object obj, string[] textureNames)
	{
		var resourcePrefix = obj.GetType ().ToString ();
		string[] texturePaths = new string[textureNames.Length];
		for (int i = 0; i < textureNames.Length; i++) {
			texturePaths [i] = resourcePrefix + "/" + textureNames [i];
		}
		return texturePaths;
	}

	static Texture2D[] loadFromPaths (ContentManager content, string[] texturePaths)
	{
		Texture2D[] textures = new Texture2D[texturePaths.Length];
		for (int i = 0; i < texturePaths.Length; i++) {
			textures [i] = content.Load<Texture2D> (texturePaths [i]);
			System.Console.WriteLine("loaded " + texturePaths[i] + " as " + textures[i]);
		}
		return textures;
	}

	public Sprite(GraphicsDeviceManager graphics, ContentManager content, object obj, params string[] textureNames) 
	: this(graphics, content, pathsForClassName (obj, textureNames)) { }
	
	public Sprite(GraphicsDeviceManager graphics, ContentManager content, params string[] texturePaths)
	: this(graphics, loadFromPaths (content, texturePaths)) { }

	public Sprite(GraphicsDeviceManager graphics, Texture2D[] textures) {
		this.graphics = graphics;
		this.textures = textures;

		width = textures[0].Width;
		height = textures[0].Height;

		quad = new Quad(Vector3.Zero, Vector3.Forward, Vector3.Down, width, height);
	}

	public void Draw()
	{
		if (isVisible) {
			BasicEffect quadEffect = new BasicEffect(graphics.GraphicsDevice);

			// We still do not have lighting implemented in the shaders
			//quadEffect.EnableDefaultLighting();
			
			quadEffect.World = Camera.main.world;
			quadEffect.View = Camera.main.view;
			quadEffect.Projection = Camera.main.projection;
			quadEffect.Texture = textures[texture_index];
			quadEffect.TextureEnabled = true;

			foreach (EffectPass pass in quadEffect.CurrentTechnique.Passes)
			{
				pass.Apply();
				
				graphics.GraphicsDevice.DrawUserIndexedPrimitives
					<VertexPositionNormalTexture>(
						PrimitiveType.TriangleList,
						quad.Vertices, 0, 4,
						quad.Indexes, 0, 2);
			}

		}
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
		var centerPoint = textures[0].Bounds.Center;
		var center = Camera.main.WorldToScreen2D(Vector2.Zero) + new Vector2(centerPoint.X, centerPoint.Y);
		return Camera.main.ScreenToWorld2D(new Vector2(center.X, center.Y));
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

//	public bool Contains(Camera camera, Vector2 position) {
//		return ScreenRect(camera).Contains(position);
//	}
//	
//	public bool Contains(Camera camera, Vector3 position) {
//		return ScreenRect(camera).Contains(new Vector2(position.x, position.y));
//	}
//
//	public void setScreenPosition(float x, float y) {
//		Vector3 pos = Camera.main.WorldToScreenPoint(gameObject.transform.position);
//		pos.x = x;
//		pos.y = y;
//		gameObject.transform.position = Camera.main.ScreenToWorldPoint(pos);
//	}
//	
//	public void setScreenPosition(int x, int y) {
//		setScreenPosition((float) x, (float) y);
//	}
//	
//	public void setScreenPosition(Vector3 position) {
//		setScreenPosition(position.x, position.y);
//	}
//	
//	public Vector3 getScreenPosition() {
//		return Camera.main.WorldToScreenPoint(gameObject.transform.position);
//	}
//	
//	/* In viewport space, 0 and 1 are the edges of the screen. */
//	public void setCenterToViewportCoord(float x, float y) {
//		var layoutpos = snapToPixel(Camera.main.ViewportToWorldPoint(new Vector3(x, y, 0.0f)));
//		gameObject.transform.position = new Vector3(layoutpos.x, layoutpos.y, gameObject.transform.position.z) - Center();
//	}
//	
//	public void setWorldPosition(float x, float y, float z) {
//		setWorldPosition(new Vector3(x, y, z));
//	}
//
//	public void setWorldPosition(Vector3 pos) {
//		gameObject.transform.position = snapToPixel(pos);
//	}
//	
//	public void setDepth(float z) {
//		gameObject.transform.Translate(Vector3.back * z);
//	}
	
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
//	public GameObject createPivotOnCenter() {
//		var parent = new GameObject("Parent of " + gameObject.name);
//		copyTransformTo(parent);
//
//		// translate the parent 2 times the sprite height.
//		// implicitly translate the sprite in the opposite direction by the same amount.
//		parent.transform.Translate(worldWidth / 2f, worldHeight / 2f, 0f);
//		gameObject.transform.parent = parent.transform;
//		
//		return parent;		
//	}
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
//	private float worldHeight {
//		// This is multiplied by two because orthographicSize is half the screen height
//		get { return height / Camera.main.pixelHeight * Camera.main.orthographicSize * 2.0f; }
//	}
//	
//	private float worldWidth {
//		// This is multiplied by two because orthographicSize is half the screen height
//		get { return width / Camera.main.pixelWidth * Camera.main.orthographicSize * 2.0f; }
//	}
//
//	private Vector3 snapToPixel(Vector3 pos) {
//		Vector3 newpos;
//		float pixelRatio = (Camera.main.orthographicSize * 2) / Camera.main.pixelHeight;
//		
//		newpos.x = Mathf.Round(pos.x / pixelRatio) * pixelRatio;
//		newpos.y = Mathf.Round(pos.y / pixelRatio) * pixelRatio;
//		newpos.z = pos.z;
//		return newpos;
//	}
//	
//	private void copyTransformTo(GameObject obj) {
//		obj.transform.parent = gameObject.transform.parent;
//		obj.transform.localRotation = gameObject.transform.localRotation;
//		obj.transform.localScale = gameObject.transform.localScale;
//		obj.transform.localPosition = gameObject.transform.localPosition;
//	}

}
