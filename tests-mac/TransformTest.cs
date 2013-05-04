using System;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace tests
{
    [TestFixture]
    public class TransformTest
    {
        readonly Vector3 anyTranslation = new Vector3(1f, 1f, 0f);
        float nonZero = 10f;
       
        Matrix flipUpsideDown = Matrix.CreateRotationZ(MathHelper.Pi);

        Transform transform;

        [SetUp]
        public void initializeTransform() {
            transform = new Transform();
        }

        [Test]
        public void localMatrixAndWorldMatrixAreIdentityWhenNew()
        {
            Assert.That(transform.localMatrix, Is.EqualTo(Matrix.Identity));
            Assert.That(transform.worldMatrix, Is.EqualTo(Matrix.Identity));
        }

        [Test]
        public void localAndWorldTranslationAreEqualWhenParentUnset()
        {
            transform.localTranslation = anyTranslation;
            Assert.That(transform.Translation, Is.EqualTo(anyTranslation));
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

            Assert.That(child.Translation, Is.EqualTo(anyTranslation + anyTranslation));
        }

        [Test]
        public void childRotatesWhenParentIsRotated()
        {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            var rotationMatrix = Matrix.CreateRotationZ(1);

            parent.rotateLocal(rotationMatrix);

            assertMatrixEquals(child.worldMatrix, rotationMatrix);
        }

        [Test]
        public void childRotatesAroundCenterWhenTurningUpsideDown()
        {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            var center = new Vector3(1, -1, 0);
            child.localTranslation = -center;
            parent.localTranslation = center;

            parent.rotateLocal(flipUpsideDown);
            
            assertVectorEquals(Vector3.Transform(Vector3.Zero, child.worldMatrix), new Vector3(2, -2, 0));
        }

        [Test]
        public void childIsTranslatedAndThenParentIsRotated()
        {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;

            var translation = new Vector3(1, -1, 0);
            child.localTranslation = translation;

            parent.rotateLocal(flipUpsideDown);
            
            assertVectorEquals(Vector3.Transform(Vector3.Zero, child.worldMatrix), -translation);
        }

        public void childIsRotatedAndThenParentIsTranslated()
        {
            var child = new Transform();
            var parent = new Transform();
            child.parent = parent;
            
            var translation = new Vector3(1, -1, 0);

            parent.localTranslation = translation;
            child.rotateLocal(flipUpsideDown);

            assertVectorEquals(Vector3.Transform(Vector3.Zero, child.worldMatrix), translation);
        }

        [Test]
        public void rotationHappensBeforeTranslation()
        {
            var child = new Transform();

            var center = new Vector3(1, -1, 0);
            child.localTranslation = center;
            
            child.rotateLocal(Quaternion.CreateFromRotationMatrix(flipUpsideDown));
            
            var transformed = Vector3.Transform(center, child.worldMatrix);
            assertVectorEquals(transformed, Vector3.Zero);
        }

        [Test]
        public void rotationDoubledWhenRotateCalledTwice()
        {
            transform.rotateLocal(Vector3.Backward, 1);
            transform.rotateLocal(Vector3.Backward, 1);
            assertMatrixEquals(transform.localMatrix, Matrix.CreateRotationZ(2));
        }

        [Test]
        public void getScaleEqualsSetScale()
        {
            var newScale = new Vector3(10f);
            transform.localScale = newScale;
            assertVectorEquals(transform.localScale, newScale);
        }

        [Test]
        public void translationPreservedWhenSettingScale()
        {
            transform.localTranslation = anyTranslation;
            var newScale = new Vector3(10f);
            transform.localScale = newScale;

            assertVectorEquals(transform.localTranslation, anyTranslation);
        }

        [Test]
        public void scaleIsTheSameWhenSettingScaleTwice()
        {
            var newScale = new Vector3(10f);
            transform.localScale = newScale;
            transform.localScale = newScale;
            assertVectorEquals(transform.localScale, newScale);
        }

        [Test]
        public void localTranslationIsTheOppositeOfPivotOffset() {
            var pivot = transform.createPivotAtOffset(nonZero, nonZero);

            assertVectorEquals(transform.localTranslation, new Vector3(-nonZero, -nonZero, 0f));
            assertVectorEquals(pivot.localTranslation, new Vector3(nonZero, nonZero, 0f));
        }

        [Test]
        public void childHasNegativeOffsetWhenTransformIsTranslated() {
            transform.localTranslation = anyTranslation;
            var pivot = transform.createPivotAtOffset(nonZero, nonZero);
            
            assertVectorEquals(transform.localTranslation, new Vector3(-nonZero, -nonZero, 0f));
            assertVectorEquals(pivot.localTranslation, anyTranslation + new Vector3(nonZero, nonZero, 0f));
        }

        [Test]
        public void childRotatesAroundPivotOffsetWhenTurningUpsideDown() {
            transform.localTranslation = anyTranslation;
            var pivot = transform.createPivotAtOffset(nonZero, nonZero);
            var nonZeroOffset = new Vector3(nonZero, nonZero, 0f);

            var localPoint = new Vector3(20, -20, 0);

            assertVectorEquals(Vector3.Transform(Vector3.Zero, transform.worldMatrix), anyTranslation);

            assertVectorEquals(Vector3.Transform(localPoint, transform.worldMatrix), localPoint + anyTranslation);

            pivot.rotateLocal(flipUpsideDown);

            assertVectorEquals(Vector3.Transform(Vector3.Zero, transform.worldMatrix), anyTranslation + nonZeroOffset + nonZeroOffset);

            assertVectorEquals(Vector3.Transform(localPoint, transform.worldMatrix), anyTranslation + nonZeroOffset - (localPoint - nonZeroOffset));

        }

        const float epsilon = 0.00001f;

        static void assertVectorEquals(Vector3 actual, Vector3 expected)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon), "X value");
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon), "Y value");
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon), "Z value");
        }

        static void assertMatrixEquals(Matrix actual, Matrix expected)
        {
            Assert.That(Matrix.ToFloatArray(actual), Is.EqualTo(Matrix.ToFloatArray(expected)).Within(epsilon));
        }

    }
}

