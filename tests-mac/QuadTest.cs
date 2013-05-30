using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace tests
{
    [TestFixture]
    public class QuadTest
    {
        float scale = 10f;
  
        [Test]
        public void upVectorDoesNotChangeRightVector()
        {
            var upScale = 2f;
            var quad = new Quad(Vector3.Zero, Vector3.Backward, new Vector3(0f, upScale, 0f), new Vector3(scale, 0, 0), 1, 1);
            Assert.That(quad.Right, Is.EqualTo(new Vector3(scale, 0, 0)));
        }

        [Test]
        public void forwardIsStillForwardWhenUpVectorIsNegative()
        {
            var upScale = 2f;
            var quad = new Quad(Vector3.Zero, Vector3.Backward, new Vector3(0f, upScale, 0f), new Vector3(1f, 0, 0), 1, 1);
            Assert.That(quad.Right, Is.EqualTo(new Vector3(1f, 0, 0)));
            Assert.That(quad.LowerRight, Is.EqualTo(new Vector3(1f, -upScale, 0f)));
        }
        const float epsilon = 0.00001f;

        static void assertVectorEquals(Vector3 actual, Vector3 expected)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon), "X value");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon), "Y value");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon), "Z value");
        }
    }
}

