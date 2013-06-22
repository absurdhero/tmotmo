using System;
using NUnit.Framework;

[TestFixture]
public class PingPongTest
{
    [Test]
    public void returnsInputWhenLessThanLength()
    {
        Assert.That(Wiggler.pingPong(0.5f, 1f), Is.EqualTo(0.5));
    }

    [Test]
    public void returnsLengthWhenEqualToLength()
    {
        Assert.That(Wiggler.pingPong(1f, 1f), Is.EqualTo(1f));
    }

    [Test]
    public void returnsZeroWhenTwiceLength()
    {
        Assert.That(Wiggler.pingPong(2f, 1f), Is.EqualTo(0f));
    }

    [Test]
    public void returnsZeroWhenZero()
    {
        Assert.That(Wiggler.pingPong(0f, 1f), Is.EqualTo(0f));
    }

    [Test]
    public void returnsNegativeWhenNegative()
    {
        Assert.That(Wiggler.pingPong(-0.5f, 1f), Is.EqualTo(0f));
    }
}

