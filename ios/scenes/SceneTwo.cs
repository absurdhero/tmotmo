
using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

class SceneTwo : Scene {
	public HospitalRoom room { get; private set; }
	
	private Wiggler wiggler;
	private TouchSensor sensor;
	
    public SceneTwo(SceneManager manager, ContentManager content, GraphicsDeviceManager graphics) : base(manager, content, graphics) {
        timeLength = 8.0f;
		room = new HospitalRoom(graphics, content, camera);
	}

	public override void Setup(float startTime) {
		room.createBackground();
		room.addZzz();
		room.addHeartRate(startTime);
		room.addFootboard();
		room.addCover();
		room.addPerson();
		
		wiggler = new Wiggler(startTime, timeLength, room.cover);
		sensor = new TouchSensor(input);
	}

	public override void Destroy() {
		wiggler.Destroy();
		// Handled by next scene
		//room.Destroy();
	}

	public override void Update () {
		wiggler.Update(Time.time);

		if (!completed) {
			if (room.touchedBed(sensor)) {
				room.openEyes();
			}
			
			room.hintWhenTouched((touched) => { if (touched == room.cover)  {
					room.removeCover();
					room.doubleHeartRate(Time.time);
					room.addSplitLine();
					endScene();
				}
			}, messagePromptCoordinator, sensor);
		}
		
		room.Update();
	}

    public override void Draw() {
        room.Draw();
    }

}
