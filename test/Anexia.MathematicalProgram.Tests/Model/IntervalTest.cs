// ------------------------------------------------------------------------------------------
//  <copyright file = "IntervalTest.cs" company = "ANEXIA® Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA® Internetdienstleistungs GmbH.All rights reserved.
//  </copyright>
//  ------------------------------------------------------------------------------------------


using Anexia.MathematicalProgram.Model.Interval;
using Anexia.MathematicalProgram.Model.Scalar;
using Anexia.MathematicalProgram.Model.Variable;

namespace Anexia.MathematicalProgram.Tests.Model;

public sealed class IntervalTest
{
    [Fact]
    public void RealIntervalKeepsInclusiveLowerBound()
    {
        var interval = new RealInterval(new RealScalar(-1.5), new RealScalar(3.25));

        Assert.Equal(new RealScalar(-1.5), interval.LowerBound);
    }

    [Fact]
    public void RealIntervalKeepsInclusiveUpperBound()
    {
        var interval = new RealInterval(new RealScalar(-1.5), new RealScalar(3.25));

        Assert.Equal(new RealScalar(3.25), interval.UpperBound);
    }

    [Theory]
    [InlineData(6, 5)]
    [InlineData(0.1, 0)]
    [InlineData(-0.1, -0.11)]
    [InlineData(double.MaxValue, double.Epsilon)]
    public void IntervalInitializingThrowsExpectedException(double left, double right) =>
        Assert.Throws<InadmissibleBoundsException<RealScalar>>(() =>
            new RealInterval(new RealScalar(left), new RealScalar(right)));

    [Fact]
    public void IntegralIntervalKeepsInclusiveLowerBound()
    {
        var interval = new IntegralInterval(new IntegerScalar(-3), new IntegerScalar(7));

        Assert.Equal(new IntegerScalar(-3), interval.LowerBound);
    }

    [Fact]
    public void IntegralIntervalKeepsInclusiveUpperBound()
    {
        var interval = new IntegralInterval(new IntegerScalar(-3), new IntegerScalar(7));

        Assert.Equal(new IntegerScalar(7), interval.UpperBound);
    }

    [Theory]
    [InlineData(6, 5)]
    [InlineData(-1, -21)]
    [InlineData(int.MaxValue, 0)]
    public void IntegralIntervalInitializingThrowsExpectedException(int left, int right) =>
        Assert.Throws<InadmissibleBoundsException<IntegerScalar>>(() =>
            new IntegralInterval(new IntegerScalar(left), new IntegerScalar(right)));

    [Fact]
    public void BinaryIntervalUsesZeroAsLowerBound()
    {
        var interval = new BinaryInterval();

        Assert.Equal(BinaryScalar.Zero, interval.LowerBound);
    }

    [Fact]
    public void BinaryIntervalUsesOneAsUpperBound()
    {
        var interval = new BinaryInterval();

        Assert.Equal(BinaryScalar.One, interval.UpperBound);
    }

    [Theory]
    [InlineData(-2.5)]
    [InlineData(0)]
    [InlineData(4.75)]
    public void PointUsesValueAsLowerBound(double value)
    {
        var point = new Point(value);

        Assert.Equal(new RealScalar(value), point.LowerBound);
    }

    [Theory]
    [InlineData(-2.5)]
    [InlineData(0)]
    [InlineData(4.75)]
    public void PointUsesValueAsUpperBound(double value)
    {
        var point = new Point(value);

        Assert.Equal(new RealScalar(value), point.UpperBound);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(4)]
    public void IntegralPointUsesValueAsLowerBound(int value)
    {
        var point = new IntegralPoint(value);

        Assert.Equal(new IntegerScalar(value), point.LowerBound);
    }

    [Theory]
    [InlineData(-2)]
    [InlineData(0)]
    [InlineData(4)]
    public void IntegralPointUsesValueAsUpperBound(int value)
    {
        var point = new IntegralPoint(value);

        Assert.Equal(new IntegerScalar(value), point.UpperBound);
    }
}
