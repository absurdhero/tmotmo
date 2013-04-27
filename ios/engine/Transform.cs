using System;
using Microsoft.Xna.Framework;

public class Transform
{
    public Matrix localMatrix = Matrix.Identity;
    public Transform parent = null;

    public Transform()
    {
    }

    public Matrix worldMatrix {
        get {
            if (hasParent())
                return Matrix.Multiply(localMatrix, parent.worldMatrix);
            return localMatrix;
        }
        set {
            // infer what the local matrix should look like based on the desired worldMatrix
            if (hasParent())
                localMatrix = Matrix.Multiply(parent.worldMatrix, value);
            else
                localMatrix = value;
        }
    }

    public bool hasParent() {
        return parent != null;
    }

    /// preserves the value of the worldMatrix while removing the direct parent
    /// this keeps the object in the same position
    public void removeImmediateParent() {
        if (!hasParent()) return;

        // apply the parent's transform to our local one
        localMatrix = localMatrix * parent.localMatrix;
        // now remove the parent from the picture
        parent = parent.parent;
    }
    public Vector3 Translation
    {
        get {
            Debug.Log("translation " + worldMatrix.Translation);
            return worldMatrix.Translation;
        }
        set {
            worldMatrix = Matrix.CreateTranslation(value) * worldMatrix;
        }
    }

    public Vector3 localTranslation
    {
        get {
            return localMatrix.Translation;
        }
        set {
            localMatrix.Translation = value;
        }
    }

    public Vector3 Forward
    {
        get { return worldMatrix.Forward; }
    }

    public Vector3 Up
    {
        get { return worldMatrix.Up; }
    }

    public Vector3 Down
    {
        get { return worldMatrix.Down; }
    }

    public Vector3 Right
    {
        get { return worldMatrix.Right; }
    }

    public void localRotate(Quaternion rotation)
    {
        localMatrix = Matrix.CreateFromQuaternion(rotation) * localMatrix;
    }

    public void localRotate(ref Vector3 backward, float angle) {
        Quaternion rotation;
        Quaternion.CreateFromAxisAngle(ref backward, -angle, out rotation);
        localRotate(rotation);
    }

    public void localScale(float scaleValue)
    {
        localMatrix = Matrix.CreateScale(scaleValue) * localMatrix;
    }
}
