using EcoRoad.Monitoring.Application.CommandServices;
using EcoRoad.Monitoring.Domain.Model;
using EcoRoad.Monitoring.Domain.Model.Aggregates;
using EcoRoad.Monitoring.Domain.Model.Commands;
using EcoRoad.Monitoring.Domain.Repositories;
using EcoRoad.Shared.Domain.Model.Exceptions;
using EcoRoad.Shared.Domain.Repositories;

namespace EcoRoad.Monitoring.Application.Internal.CommandServices;

/// <summary>
/// Handles the configuration of threshold profiles.
/// </summary>
public class ThresholdProfileCommandService : IThresholdProfileCommandService
{
    private readonly IThresholdProfileRepository _profiles;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<ThresholdProfileCommandService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ThresholdProfileCommandService" /> class.
    /// </summary>
    /// <param name="profiles">Repository of threshold profiles.</param>
    /// <param name="unitOfWork">Unit of work that persists the changes.</param>
    /// <param name="logger">Logger used to trace the handled commands.</param>
    public ThresholdProfileCommandService(
        IThresholdProfileRepository profiles,
        IUnitOfWork unitOfWork,
        ILogger<ThresholdProfileCommandService> logger)
    {
        _profiles = profiles;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<ThresholdProfile> Handle(
        ConfigureThresholdProfileCommand command,
        CancellationToken cancellationToken = default)
    {
        var profile = new ThresholdProfile(command.Name, command.ParameterType, command.Limits);
        if (await _profiles.FindByParameterTypeAsync(profile.ParameterType, cancellationToken) != null)
        {
            throw new MonitoringRuleException(
                "PROFILE_ALREADY_CONFIGURED",
                "The parameter already has a threshold profile. Update its thresholds instead.",
                DomainErrorKind.Conflict);
        }

        await _profiles.AddAsync(profile, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Threshold profile {ProfileId} configured for {Parameter}.", profile.Id, profile.ParameterType);
        return profile;
    }

    /// <inheritdoc />
    public async Task<ThresholdProfile> Handle(AssignThresholdsCommand command, CancellationToken cancellationToken = default)
    {
        var profile = await _profiles.FindByIdAsync(command.ThresholdProfileId, cancellationToken)
            ?? throw new MonitoringRuleException(
                "PROFILE_NOT_FOUND",
                "The threshold profile was not found.",
                DomainErrorKind.NotFound);

        profile.AssignEnvironmentalThresholds(command.Limits);
        await _unitOfWork.CompleteAsync(cancellationToken);

        _logger.LogInformation("Thresholds of profile {ProfileId} updated.", profile.Id);
        return profile;
    }
}
