// ------------------------------------------------------------------------------------------
//  <copyright file = "ResultHandlingTest.cs" company = "ANEXIA Internetdienstleistungs GmbH">
//  Copyright (c) ANEXIA Internetdienstleistungs GmbH. All rights reserved.
//  </copyright>
// ------------------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using Anexia.MathematicalProgram.Model.Scalar;
using Anexia.MathematicalProgram.Model.Variable;
using Anexia.MathematicalProgram.Result;
using Anexia.MathematicalProgram.Solve;
using Google.OrTools.ModelBuilder;
using Google.OrTools.Sat;
using Gurobi;

namespace Anexia.MathematicalProgram.Tests.Result;

public sealed class ResultHandlingTest
{
    public static IEnumerable<object?[]> OrToolsModelBuilderStatusMappings()
    {
        yield return [SolveStatus.INFEASIBLE, SolverResultStatus.Infeasible, false];
        yield return [SolveStatus.UNBOUNDED, SolverResultStatus.Unbounded, null];
        yield return [SolveStatus.ABNORMAL, SolverResultStatus.Abnormal, null];
        yield return [SolveStatus.NOT_SOLVED, SolverResultStatus.NotSolved, null];
        yield return [SolveStatus.MODEL_INVALID, SolverResultStatus.ModelInvalid, null];
        yield return [SolveStatus.MODEL_IS_VALID, SolverResultStatus.ModelIsValid, null];
        yield return [SolveStatus.CANCELLED_BY_USER, SolverResultStatus.CancelledByUser, null];
        yield return [SolveStatus.UNKNOWN_STATUS, SolverResultStatus.UnknownStatus, null];
        yield return [SolveStatus.INVALID_SOLVER_PARAMETERS, SolverResultStatus.InvalidSolverParameters, null];
        yield return [SolveStatus.SOLVER_TYPE_UNAVAILABLE, SolverResultStatus.SolverTypeUnavailable, null];
        yield return [SolveStatus.INCOMPATIBLE_OPTIONS, SolverResultStatus.IncompatibleOptions, null];
    }

    public static IEnumerable<object?[]> GurobiNoSolutionStatusMappings()
    {
        yield return [GRB.Status.INFEASIBLE, SolverResultStatus.Infeasible, false];
        yield return [GRB.Status.UNBOUNDED, SolverResultStatus.Unbounded, null];
        yield return [GRB.Status.INTERRUPTED, SolverResultStatus.CancelledByUser, null];
        yield return [GRB.Status.INF_OR_UNBD, SolverResultStatus.InfOrUnbound, null];
        yield return [GRB.Status.TIME_LIMIT, SolverResultStatus.Timelimit, null];
    }

    public static IEnumerable<object[]> GurobiSolutionStatusMappings()
    {
        yield return [GRB.Status.OPTIMAL, SolverResultStatus.Optimal, true];
        yield return [GRB.Status.SUBOPTIMAL, SolverResultStatus.Feasible, false];
        yield return [GRB.Status.TIME_LIMIT, SolverResultStatus.Timelimit, false];
        yield return [GRB.Status.INTERRUPTED, SolverResultStatus.CancelledByUser, false];
        yield return [GRB.Status.MEM_LIMIT, SolverResultStatus.UnknownStatus, false];
    }

    [Theory]
    [MemberData(nameof(OrToolsModelBuilderStatusMappings))]
    public void HandleOrToolsModelBuilderStatusMapsNonSolutionStatuses(
        SolveStatus solveStatus,
        SolverResultStatus expectedStatus,
        bool? expectedFeasible)
    {
        var result = ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            solveStatus,
            switchedToDefaultSolver: true);

        Assert.Equal(
            EmptySolverResult(expectedStatus, switchedToDefaultSolver: true, expectedFeasible),
            result);
    }

    [Theory]
    [MemberData(nameof(GurobiNoSolutionStatusMappings))]
    public void HandleGurobiStatusMapsNonSolutionStatuses(
        int gurobiStatus,
        SolverResultStatus expectedStatus,
        bool? expectedFeasible)
    {
        var result = ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            gurobiStatus,
            switchedToDefaultSolver: true);

        Assert.Equal(
            EmptySolverResult(expectedStatus, switchedToDefaultSolver: true, expectedFeasible),
            result);
    }

    [Fact]
    public void HandleOrToolsModelBuilderOptimalStatusBuildsResultWithGap()
    {
        var result = ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            SolveStatus.OPTIMAL,
            switchedToDefaultSolver: true,
            objectiveValue: 12,
            bestBound: 9);

        Assert.Equal(
            new SolverResult<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                EmptySolutionValues,
                new ObjectiveValue(12),
                new IsFeasible(true),
                new IsOptimal(true),
                new OptimalityGap(0.25),
                SolverResultStatus.Optimal,
                SwitchedToDefaultSolver: true),
            result);
    }

    [Fact]
    public void HandleCpOptimalStatusReturnsZeroGapWhenObjectiveAndBestBoundAreZero()
    {
        var result = ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            CpSolverStatus.Optimal,
            objectiveValue: 0,
            bestBound: 0);

        Assert.Equal(
            new SolverResult<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                EmptySolutionValues,
                new ObjectiveValue(0),
                new IsFeasible(true),
                new IsOptimal(true),
                new OptimalityGap(0),
                SolverResultStatus.Optimal,
                SwitchedToDefaultSolver: false),
            result);
    }

    [Theory]
    [MemberData(nameof(GurobiSolutionStatusMappings))]
    public void HandleGurobiSolutionStatusBuildsExpectedResult(
        int gurobiStatus,
        SolverResultStatus expectedStatus,
        bool expectedOptimal)
    {
        var result = ResultHandling.HandleGurobi<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            gurobiStatus,
            objectiveValue: 20,
            bestBound: 15);

        Assert.Equal(
            new SolverResult<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                EmptySolutionValues,
                new ObjectiveValue(20),
                new IsFeasible(true),
                new IsOptimal(expectedOptimal),
                new OptimalityGap(0.25),
                expectedStatus,
                SwitchedToDefaultSolver: false),
            result);
    }

    [Fact]
    public void HandleGurobiInfeasibleStatusBuildsInfeasibleResult()
    {
        var result = ResultHandling.HandleGurobi<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            GRB.Status.INFEASIBLE,
            objectiveValue: 20,
            bestBound: 15);

        Assert.Equal(
            EmptySolverResult(SolverResultStatus.Infeasible, switchedToDefaultSolver: false, isFeasible: false),
            result);
    }

    [Fact]
    public void HandleGurobiUnboundedStatusBuildsNonFeasibleResultWhenBoundsArePresent()
    {
        var result = ResultHandling.HandleGurobi<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            GRB.Status.UNBOUNDED,
            objectiveValue: 20,
            bestBound: 15);

        Assert.Equal(
            EmptySolverResult(SolverResultStatus.Unbounded, switchedToDefaultSolver: false, isFeasible: null),
            result);
    }

    [Fact]
    public void HandleOptimalStatusWithoutObjectiveValueThrows()
    {
        var exception = Assert.Throws<MathematicalProgramException>(() =>
            ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                SolveStatus.OPTIMAL,
                switchedToDefaultSolver: false));

        Assert.Equal("Mathematical program could not be solved.", exception.Message);
    }

    [Fact]
    public void HandleGurobiSolutionStatusWithoutBestBoundThrows()
    {
        var exception = Assert.Throws<MathematicalProgramException>(() =>
            ResultHandling.HandleGurobi<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                GRB.Status.OPTIMAL,
                objectiveValue: 1));

        Assert.Equal("Mathematical program could not be solved.", exception.Message);
    }

    public static IEnumerable<object[]> GurobiSolutionStatusesRequiringObjectiveAndBound()
    {
        yield return [GRB.Status.SUBOPTIMAL];
        yield return [GRB.Status.TIME_LIMIT];
        yield return [GRB.Status.INTERRUPTED];
        yield return [GRB.Status.MEM_LIMIT];
        yield return [GRB.Status.UNBOUNDED];
    }

    [Theory]
    [MemberData(nameof(GurobiSolutionStatusesRequiringObjectiveAndBound))]
    public void HandleGurobiSolutionStatusWithoutObjectiveValueThrows(int gurobiStatus)
    {
        var exception = Assert.Throws<MathematicalProgramException>(() =>
            ResultHandling.HandleGurobi<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                gurobiStatus,
                bestBound: 1));

        Assert.Equal("Mathematical program could not be solved.", exception.Message);
    }

    [Fact]
    public void HandleGurobiIntStatusOptimalWithObjectiveBuildsOptimalResultWithGap()
    {
        var result = ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
            GRB.Status.OPTIMAL,
            switchedToDefaultSolver: false,
            objectiveValue: 12,
            bestBound: 9);

        Assert.Equal(
            new SolverResult<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                EmptySolutionValues,
                new ObjectiveValue(12),
                new IsFeasible(true),
                new IsOptimal(true),
                new OptimalityGap(0.25),
                SolverResultStatus.Optimal,
                SwitchedToDefaultSolver: false),
            result);
    }

    [Fact]
    public void HandleGurobiIntStatusOptimalWithoutObjectiveValueThrows()
    {
        var exception = Assert.Throws<MathematicalProgramException>(() =>
            ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                GRB.Status.OPTIMAL,
                switchedToDefaultSolver: false));

        Assert.Equal("Mathematical program could not be solved.", exception.Message);
    }

    [Fact]
    public void HandleUnknownStatusThrows()
    {
        var exception = Assert.Throws<MathematicalProgramException>(() =>
            ResultHandling.Handle<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                (SolveStatus)int.MaxValue,
                switchedToDefaultSolver: false));

        Assert.Equal("Unknown result status in solver. 2147483647", exception.Message);
    }

    [Fact]
    public void HandleGurobiUnknownStatusThrows()
    {
        var exception = Assert.Throws<MathematicalProgramException>(() =>
            ResultHandling.HandleGurobi<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar>(
                int.MaxValue,
                objectiveValue: 1,
                bestBound: 1));

        Assert.Equal("Unknown result status in solver. 2147483647", exception.Message);
    }

    private static SolutionValues<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> EmptySolutionValues =>
        new(ReadOnlyDictionary<IIntegerVariable<IRealScalar>, RealScalar>.Empty);

    private static SolverResult<IIntegerVariable<IRealScalar>, RealScalar, IRealScalar> EmptySolverResult(
        SolverResultStatus status,
        bool switchedToDefaultSolver,
        bool? isFeasible) =>
        new(
            EmptySolutionValues,
            ObjectiveValue: null,
            isFeasible is null ? null : new IsFeasible(isFeasible.Value),
            new IsOptimal(false),
            OptimalityGap: null,
            status,
            switchedToDefaultSolver);
}
