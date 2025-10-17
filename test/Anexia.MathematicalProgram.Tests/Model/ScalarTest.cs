// ------------------------------------------------------------------------------------------
//  <copyright file = "ScalarTest.cs" company = "ANEXIA Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using Anexia.MathematicalProgram.Model.Scalar;

namespace Anexia.MathematicalProgram.Tests.Model;

public sealed class ScalarTest
{
    [Fact]
    public void RealScalarAddReturnsSum()
    {
        Assert.Equal(new RealScalar(9.5), new RealScalar(7.5).Add(new RealScalar(2)));
    }

    [Fact]
    public void RealScalarSubtractReturnsDifference()
    {
        Assert.Equal(new RealScalar(5.5), new RealScalar(7.5).Subtract(new RealScalar(2)));
    }

    [Fact]
    public void RealScalarAddViaInterfaceReturnsSum()
    {
        IRealScalar right = new RealScalar(2);

        Assert.Equal(9.5, new RealScalar(7.5).Add(right).Value);
    }

    [Fact]
    public void RealScalarSubtractViaInterfaceReturnsDifference()
    {
        IRealScalar right = new RealScalar(2);

        Assert.Equal(5.5, new RealScalar(7.5).Subtract(right).Value);
    }

    [Fact]
    public void RealScalarPlusOperatorReturnsSum()
    {
        Assert.Equal(new RealScalar(9.5), new RealScalar(7.5) + new RealScalar(2));
    }

    [Fact]
    public void RealScalarMinusOperatorReturnsDifference()
    {
        Assert.Equal(new RealScalar(5.5), new RealScalar(7.5) - new RealScalar(2));
    }

    [Fact]
    public void IntegerScalarAddReturnsSum()
    {
        Assert.Equal(new IntegerScalar(9), new IntegerScalar(7).Add(new IntegerScalar(2)));
    }

    [Fact]
    public void IntegerScalarSubtractReturnsDifference()
    {
        Assert.Equal(new IntegerScalar(5), new IntegerScalar(7).Subtract(new IntegerScalar(2)));
    }

    [Fact]
    public void IntegerScalarAddViaIntegerInterfaceReturnsSum()
    {
        IIntegerScalar right = new IntegerScalar(2);

        Assert.Equal(9, new IntegerScalar(7).Add(right).Value);
    }

    [Fact]
    public void IntegerScalarSubtractViaIntegerInterfaceReturnsDifference()
    {
        IIntegerScalar right = new IntegerScalar(2);

        Assert.Equal(5, new IntegerScalar(7).Subtract(right).Value);
    }

    [Fact]
    public void IntegerScalarAddRealScalarReturnsRealSum()
    {
        IRealScalar right = new RealScalar(2.5);

        Assert.Equal(9.5, new IntegerScalar(7).Add(right).Value);
    }

    [Fact]
    public void IntegerScalarSubtractRealScalarReturnsRealDifference()
    {
        IRealScalar right = new RealScalar(2.5);

        Assert.Equal(4.5, new IntegerScalar(7).Subtract(right).Value);
    }

    [Fact]
    public void IntegerScalarPlusOperatorReturnsSum()
    {
        Assert.Equal(new IntegerScalar(9), new IntegerScalar(7) + new IntegerScalar(2));
    }

    [Fact]
    public void IntegerScalarMinusOperatorReturnsDifference()
    {
        Assert.Equal(new IntegerScalar(5), new IntegerScalar(7) - new IntegerScalar(2));
    }

    [Fact]
    public void BinaryScalarAddEvenIntegerScalarKeepsValue()
    {
        IIntegerScalar even = new IntegerScalar(4);

        Assert.Equal(1, BinaryScalar.One.Add(even).Value);
    }

    [Fact]
    public void BinaryScalarAddOddIntegerScalarFlipsValue()
    {
        IIntegerScalar odd = new IntegerScalar(3);

        Assert.Equal(0, BinaryScalar.One.Add(odd).Value);
    }

    [Fact]
    public void BinaryScalarSubtractEvenIntegerScalarKeepsValue()
    {
        IIntegerScalar even = new IntegerScalar(4);

        Assert.Equal(1, BinaryScalar.One.Subtract(even).Value);
    }

    [Fact]
    public void BinaryScalarSubtractOddIntegerScalarFlipsValue()
    {
        IIntegerScalar odd = new IntegerScalar(3);

        Assert.Equal(1, BinaryScalar.Zero.Subtract(odd).Value);
    }
}
