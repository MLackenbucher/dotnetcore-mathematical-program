// ------------------------------------------------------------------------------------------
//  <copyright file = "ConstraintTest.cs" company = "ANEXIA Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using Anexia.MathematicalProgram.Model;
using Anexia.MathematicalProgram.Model.Expression;
using Anexia.MathematicalProgram.Model.Interval;
using Anexia.MathematicalProgram.Model.Scalar;
using Anexia.MathematicalProgram.Model.Variable;
using static Anexia.MathematicalProgram.Tests.Factory.WeightedSumFactory;

namespace Anexia.MathematicalProgram.Tests.Model;

public sealed class ConstraintTest
{
    [Fact]
    public void ConstraintKeepsWeightedSumIntervalAndName()
    {
        var (weightedSum, interval) = CreateWeightedSumAndInterval();

        var constraint = new Constraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            weightedSum,
            interval,
            "capacity");

        Assert.Equal(
            new Constraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(weightedSum, interval, "capacity"),
            constraint);
    }

    [Fact]
    public void ConstraintsConstructorMaterializesEnumerable()
    {
        var constraint = CreateConstraint("c1");
        var source = new List<IConstraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>> { constraint };
        var constraints = new Constraints<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            source.Where(_ => true));

        source.Clear();

        Assert.Equal(new[] { constraint as IConstraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> },
            [.. constraints]);
    }

    [Fact]
    public void AddReturnsNewConstraintsCollectionWithoutChangingOriginal()
    {
        var first = CreateConstraint("c1");
        var second = CreateConstraint("c2");
        var constraints = new Constraints<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(first);

        var updated = constraints.Add(second);

        Assert.Equal(
            new[]
            {
                first as IConstraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>,
                first as IConstraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>,
                second as IConstraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>
            },
            [.. constraints, ..updated]);
    }

    private static Constraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> CreateConstraint(string name)
    {
        var (weightedSum, interval) = CreateWeightedSumAndInterval();

        return new Constraint<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            weightedSum,
            interval,
            name);
    }

    private static (IWeightedSum<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> WeightedSum,
        IInterval<IRealScalar> Interval) CreateWeightedSumAndInterval()
    {
        var model = new OptimizationModel<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>();
        var variable = model.NewVariable<IntegerVariable<IRealScalar>>(new RealInterval(0, 10), "x");

        return (WeightedSum((variable, 2)), new RealInterval(-4, 8));
    }
}