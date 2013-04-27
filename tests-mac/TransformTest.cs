using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace tests
{
    [TestFixture()]
    public class TransformTest
    {
        [Test()]
        public void localMatrixIsSameAsWorldMatrixWhenParentUnset()
        {
            Transform transform = new Transform();
            Assert.That(transform.localMatrix, Is.EqualTo(transform.worldMatrix));

            var expectedTranslation = Vector3.Up;
            transform.localTranslation = expectedTranslation;
            Assert.That(transform.localMatrix, Is.EqualTo(transform.worldMatrix));
        }
    }
}

