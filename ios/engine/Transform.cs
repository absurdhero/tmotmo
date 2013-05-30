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
            return worldMatrix.Translation;
        }
        set {
            var newWorldMatrix = worldMatrix;
            newWorldMatrix.Translation = value;
            worldMatrix = newWorldMatrix;
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

    public void rotateLocal(Quaternion rotation)
    {
        localMatrix = Matrix.CreateFromQuaternion(rotation) * localMatrix;
    }

    public void rotateLocal(Matrix rotationMatrix)
    {
        localMatrix = rotationMatrix * localMatrix;
    }

    public void rotateLocal(Vector3 backward, float angle) {
        rotateLocal(Matrix.CreateFromAxisAngle(backward, angle));
    }

    public Vector3 localScale
    {
        get {
            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            localMatrix.Decompose(out scale, out rotation, out translation);
            return scale;
        }
        set {
            Vector3 scale;
            Quaternion rotation;
            Vector3 translation;
            localMatrix.Decompose(out scale, out rotation, out translation);

            localMatrix = Matrix.CreateScale(value) * Matrix.CreateFromQuaternion(rotation) * Matrix.CreateTranslation(translation);
        }
    }

    public Transform createPivotAtOffset(float x, float y) {
        var pivot = new Transform();

        var translation = new Vector3(x, y, 0f);

        // move the pivot in the other direction
        pivot.localTranslation = translation + localTranslation;
        localTranslation = -translation;
        
        // insert the pivot as a parent
        pivot.parent = this.parent;
        this.parent = pivot;
        
        return pivot;
    }
}

