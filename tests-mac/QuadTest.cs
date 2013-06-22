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

        [Test]
        public void cropToRectangle()
        {
            var quadWidth = 10;
            var quadHeight = 10;
            var quad = new Quad(Vector3.Zero, Vector3.Backward, Vector3.Up, Vector3.Right, quadWidth, quadHeight);

            const int cropSize = 4;
            quad.cropTextureToRectangle(new Rectangle(2, 2, cropSize, cropSize), quadWidth, quadHeight);

            var upperLeft = quad.Vertices[1];
            var lowerRight = quad.Vertices[2];

            Assert.That(upperLeft.TextureCoordinate, Is.EqualTo(new Vector2(2 / (float) quadWidth, 2 / (float) quadHeight)));
            Assert.That(lowerRight.TextureCoordinate, Is.EqualTo(new Vector2((2 + cropSize) / (float) quadWidth, (2 + cropSize) / (float) quadHeight)));
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

