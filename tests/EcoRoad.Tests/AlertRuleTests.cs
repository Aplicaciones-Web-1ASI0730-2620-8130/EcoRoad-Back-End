using EcoRoad.Alerting.Domain.Model.Aggregates;
using EcoRoad.Alerting.Domain.Model.ValueObjects;
using Xunit;

namespace EcoRoad.Tests.Alerting;

public class AlertRuleTests
{
    private static AlertRule CreateRule(double criticalExcess = 10)
    {
        return new AlertRule(
            "PM10",
            "ug/m3",
            TimeBand.Daytime,
            40,
            criticalExcess
        );
    }

    [Theory]
    [InlineData(35)]
    [InlineData(40)]
    public void Evaluate_WithinLimit_ReturnsNoDeviation(
        double measuredValue)
    {
        var rule = CreateRule();

        var result = rule.Evaluate(
            measuredValue,
            TimeBand.Daytime
        );

        Assert.False(result.IsDeviation);
        Assert.False(result.IsCritical);
        Assert.Equal(measuredValue, result.MeasuredValue);
        Assert.Equal(40, result.LimitValue);
    }

    [Fact]
    public void Evaluate_AboveLimit_ReturnsNonCriticalDeviation()
    {
        var rule = CreateRule();

        var result = rule.Evaluate(45, TimeBand.Daytime);

        Assert.True(result.IsDeviation);
        Assert.False(result.IsCritical);
    }

    [Fact]
    public void Evaluate_AtCriticalThreshold_ReturnsCriticalDeviation()
    {
        var rule = CreateRule();

        var result = rule.Evaluate(50, TimeBand.Daytime);

        Assert.True(result.IsDeviation);
        Assert.True(result.IsCritical);
    }

    [Fact]
    public void Evaluate_DifferentTimeBand_ReturnsNoDeviation()
    {
        var rule = CreateRule();

        var result = rule.Evaluate(60, TimeBand.Nighttime);

        Assert.False(result.IsDeviation);
        Assert.False(result.IsCritical);
    }

    [Fact]
    public void Evaluate_ZeroCriticalExcess_MarksDeviationAsCritical()
    {
        var rule = CreateRule(criticalExcess: 0);

        var result = rule.Evaluate(41, TimeBand.Daytime);

        Assert.True(result.IsDeviation);
        Assert.True(result.IsCritical);
    }

    [Fact]
    public void Evaluate_NaN_ThrowsValidationException()
    {
        var rule = CreateRule();

        var exception = Assert.ThrowsAny<Exception>(
            () => rule.Evaluate(double.NaN, TimeBand.Daytime)
        );

        Assert.Contains("finite number", exception.Message);
    }

    [Fact]
    public void Constructor_TrimsParameterAndUnit()
    {
        var rule = new AlertRule(
            " PM10 ",
            " ug/m3 ",
            TimeBand.Daytime,
            40,
            10
        );

        Assert.Equal("PM10", rule.Parameter);
        Assert.Equal("ug/m3", rule.Unit);
    }

    [Fact]
    public void Constructor_NegativeCriticalExcess_ThrowsException()
    {
        var exception = Assert.ThrowsAny<Exception>(
            () => new AlertRule(
                "PM10",
                "ug/m3",
                TimeBand.Daytime,
                40,
                -5
            )
        );

        Assert.Contains("cannot be negative", exception.Message);
    }
}