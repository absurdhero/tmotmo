using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace tests
{
    [TestFixture]
    public class TransformTest
    {
        readonly Vector3 anyTranslation = Vector3.Up;

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

            parent.localRotate(Quaternion.CreateFromRotationMatrix(rotationMatrix));

            Assert.That(Matrix.ToFloatArray(child.worldMatrix), Is.EqualTo(Matrix.ToFloatArray(rotationMatrix)).Within(0.000001));
        }

    }
}

