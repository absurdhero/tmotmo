using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace tests
{
    [TestFixture]
    public class TransformTest
    {
        readonly Vector3 anyTranslation = Vector3.Up;
        Matrix flipUpsideDown = Matrix.CreateRotationZ(MathHelper.Pi);

        [Test]
        public void localMatrixAndWorldMatrixAreIdentityWhenNew()
        {
            Transform transform = new Transform();
            Assert.That(transform.localMatrix, Is.EqualTo(Matrix.Identity));
			Assert.That(transform.worldMatrix, Is.EqualTo(Matrix.Identity));
		}

		[Test]
		public void localAndWorldTranslationAreEqualWhenParentUnset()
		{
			Transform transform = new Transform();

            transform.localTranslation = anyTranslation;
			Assert.That (transform.Translation, Is.EqualTo(anyTranslation));
            Assert.That(transform.localMatrix, Is.EqualTo(transform.worldMatrix));
        }

        [Test]
        public void worldTranslationisAddedToLocalTranslation()
        {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            child.localTranslation = anyTranslation;
            parent.localTranslation = anyTranslation;

            Assert.That (child.Translation, Is.EqualTo(anyTranslation + anyTranslation));
        }

        [Test]
        public void childRotatesWhenParentIsRotated() {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            var rotationMatrix = Matrix.CreateRotationZ(1);

            parent.localRotate(rotationMatrix);

            Assert.That(Matrix.ToFloatArray(child.worldMatrix), Is.EqualTo(Matrix.ToFloatArray(rotationMatrix)).Within(0.000001));
        }

        [Test]
        public void childRotatesAroundCenterWhenTurningUpsideDown() {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            var center = new Vector3(1, -1, 0);
            child.localTranslation = -center;
            parent.localTranslation = center;

            parent.localRotate(flipUpsideDown);
            
            assertVectorsEqual(Vector3.Transform(Vector3.Zero, child.worldMatrix), new Vector3(2, -2, 0));
        }

        [Test]
        public void parentRotatedAndThenChildIsTranslated() {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            var center = new Vector3(1, -1, 0);
            child.localTranslation = center;

            parent.localRotate(flipUpsideDown);
            
            assertVectorsEqual(Vector3.Transform(Vector3.Zero, child.worldMatrix), new Vector3(-1, 1, 0));
        }

        [Test]
        public void rotationHappensBeforeTranslation() {
            var child = new Transform();

            var center = new Vector3(1, -1, 0);
            child.localTranslation = center;
            
            child.localRotate(Quaternion.CreateFromRotationMatrix(flipUpsideDown));
            
            var transformed = Vector3.Transform(center, child.worldMatrix);
            assertVectorsEqual(transformed, Vector3.Zero);
        }

        const float epsilon = 0.000001f;

        static void assertVectorsEqual(Vector3 actual, Vector3 expected)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon), "X value");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon), "Y value");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon), "Z value");
        }
    }
}

