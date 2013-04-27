using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace tests
{
    [TestFixture]
    public class TransformTest
    {
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
            var anyTranslation = Vector3.Up;

            transform.localTranslation = anyTranslation;
			Assert.That (transform.Translation, Is.EqualTo(anyTranslation));
            Assert.That(transform.localMatrix, Is.EqualTo(transform.worldMatrix));
        }
    }
}

