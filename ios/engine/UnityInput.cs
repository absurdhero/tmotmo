using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace TouchInput
{
    public class UnityInput
    {
        // the game sets this at the beginning of each frame update
        public static TouchCollection touchCollection { get; set; }

        public UnityInput()
        {
        
        }
    
        public int touchCount
        {
            get { return touchCollection.Count; }
        }
    
        public Vector3 mousePosition
        {
            get { return Vector3.Zero; }
        }
        
        public Touch GetTouch(int index)
        {
            var touch = new Touch();
            touch.phase = Touch.PhaseFromState(touchCollection[index].State);
            touch.position = touchCollection[index].Position;
            return touch;
        }
    
        public bool hasMoved(int touchIndex)
        {
            return GetTouch(touchIndex).phase == TouchPhase.Moved;
        }
    
        public bool GetMouseButtonUp(int index)
        {
            return false;
        }

        public bool GetMouseButton(int index)
        {
            return false;
        }
    }

    public class Touch
    {
        public TouchPhase phase;
        public Vector2 position;
        
        public static TouchPhase PhaseFromState(TouchLocationState touchState)
        {
            switch (touchState)
            {
                case TouchLocationState.Moved:
                    return TouchPhase.Moved;
                case TouchLocationState.Pressed:
                    return TouchPhase.Began;
                case TouchLocationState.Released:
                    return TouchPhase.Ended;
                case TouchLocationState.Invalid:
                    return TouchPhase.Canceled;
            }
            // can't get here but must satisfy the compiler
            return TouchPhase.Canceled;
        }
    }
    
    public enum TouchPhase
    {
        Began,
        Moved,
        Ended,
        Canceled
    }

}