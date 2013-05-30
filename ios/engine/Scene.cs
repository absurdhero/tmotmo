using System;
using System.Collections;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using TouchInput;

public abstract class Scene : MarshalByRefObject {
	protected float timeLength = 0.0f;
	protected Camera camera;
	protected SceneManager sceneManager;
    protected ContentManager content;
    protected GraphicsDeviceManager graphics;

	public float rewindTime { get; protected set; }

	protected UnityInput input;

	public virtual float TimeLength() {
		return timeLength;
	}

	/// Indicates that the puzzle has been solved
	public bool solved { get; private set; }

	public void solvedScene() {
		solved = true;
	}

	/// Whether the scene will end once the current loop completes
	public bool completed { get; private set; }
	
	public MessagePromptCoordinator messagePromptCoordinator { get { return sceneManager.messagePromptCoordinator; } }
	
	public void endScene() {
		solved = true;
		completed = true;
	}

	/// OK to spend time unloading unused resources from prior scenes.
	/// Set this to false if transition between scenes must be perfectly smooth
	public bool permitUnloadResources { get; protected set; }

    public Scene(SceneManager manager, ContentManager content, GraphicsDeviceManager graphics) {
		sceneManager = manager;
        this.content = content;
        this.graphics = graphics;
        completed = false;
		permitUnloadResources = true;
		camera = Camera.main;
		input = new UnityInput();
	}

	public virtual void LoadAssets() {}
	public abstract void Setup(float startTime);
	public abstract void Update();
    public abstract void Draw();
	public abstract void Destroy();
	
	public virtual void Transition() {
		sceneManager.NextScene();
	}
	
	protected void rewindLoop(float seconds) {
		rewindTime = seconds;
	}

}
