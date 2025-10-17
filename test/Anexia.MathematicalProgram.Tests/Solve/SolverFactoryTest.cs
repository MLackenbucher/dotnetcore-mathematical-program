// ------------------------------------------------------------------------------------------
//  <copyright file = "SolverFactoryTest.cs" company = "ANEXIA Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using Anexia.MathematicalProgram.Solve;
using Anexia.MathematicalProgram.SolverConfiguration;

namespace Anexia.MathematicalProgram.Tests.Solve;

public sealed class SolverFactoryTest
{
    [Theory]
    [InlineData(IlpSolverType.GurobiNativeIntegerProgramming)]
    [InlineData(IlpSolverType.GurobiIntegerProgramming)]
    [InlineData(IlpSolverType.Scip)]
    [InlineData(IlpSolverType.HiGhs)]
    public void SolverForIlpTypeReturnsIlpSolver(IlpSolverType solverType)
    {
        var solver = SolverFactory.SolverFor(solverType);

        Assert.IsType<IlpSolver>(solver);
    }

    [Fact]
    public void SolverForCbcReturnsIlpCbcSolver()
    {
#pragma warning disable CS0618 // CBC is obsolete but still resolved by the factory.
        var solver = SolverFactory.SolverFor(IlpSolverType.CbcIntegerProgramming);

        Assert.IsType<IlpCbcSolver>(solver);
#pragma warning restore CS0618
    }

    [Fact]
    public void SolverForUnknownIlpTypeThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            SolverFactory.SolverFor((IlpSolverType)int.MaxValue));

        Assert.Equal("solverType", exception.ParamName);
    }

    [Theory]
    [InlineData(LpSolverType.Glop)]
    [InlineData(LpSolverType.Scip)]
    [InlineData(LpSolverType.GurobiMixedIntegerProgramming)]
    public void SolverForLpTypeReturnsLpSolver(LpSolverType solverType)
    {
        var solver = SolverFactory.SolverFor(solverType);

        Assert.IsType<LpSolver>(solver);
    }

    [Fact]
    public void SolverForUnknownLpTypeThrows()
    {
        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            SolverFactory.SolverFor((LpSolverType)int.MaxValue));

        Assert.Equal("solverType", exception.ParamName);
    }

    [Fact]
    public void NewCpSolverReturnsConstraintProgrammingSolver()
    {
        var solver = SolverFactory.NewCpSolver();

        Assert.IsType<ConstraintProgrammingSolver>(solver);
    }
}
