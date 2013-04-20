using System;

using Microsoft.Xna.Framework;

public class UnityInput {
	public UnityInput () {
		
	}
	
	public int touchCount {
		get { return 0; }
	}
	
	public Vector3 mousePosition {
		get { return Vector3.Zero; }
	}
		
	public Touch GetTouch(int index) {
        return new Touch();
	}
	
	public bool hasMoved(int touchIndex) {
		return GetTouch(touchIndex).phase == TouchPhase.Moved;
	}
	
	public bool GetMouseButtonUp(int index) {
        return false;
	}

	public bool GetMouseButton(int index) {
        return false;
	}

    public class Touch {
        public TouchPhase phase;
    }

    public enum TouchPhase {
        Began,
        Moved,
        Stationary,
        Ended,
        Canceled
    }
}
