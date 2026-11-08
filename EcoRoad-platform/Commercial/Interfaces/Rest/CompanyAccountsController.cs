using EcoRoad.Commercial.Application.CommandServices;
using EcoRoad.Commercial.Application.QueryServices;
using EcoRoad.Commercial.Domain.Model;
using EcoRoad.Commercial.Domain.Model.Commands;
using EcoRoad.Commercial.Domain.Model.Queries;
using EcoRoad.Commercial.Interfaces.Rest.Resources;
using EcoRoad.Commercial.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EcoRoad.Commercial.Interfaces.Rest;

[ApiController]
[Route("api/company-accounts")]
public class CompanyAccountsController : ControllerBase
{
    private readonly ICommercialCommandService _commands;
    private readonly ICommercialQueryService _queries;

    public CompanyAccountsController(ICommercialCommandService commands, ICommercialQueryService queries)
    {
        _commands = commands;
        _queries = queries;
    }

    [HttpPost]
    public async Task<IActionResult> RegisterCompanyAccount(
        [FromBody] RegisterCompanyAccountResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = RegisterCompanyAccountCommandFromResourceAssembler.ToCommandFromResource(resource);
            var account = await _commands.Handle(command, cancellationToken);
            var body = CompanyAccountResourceFromEntityAssembler.ToResourceFromEntity(account);
            return CreatedAtAction(nameof(GetSubscription), new { companyAccountId = account.Id }, body);
        }
        catch (CommercialRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpGet("{companyAccountId}/subscription")]
    public async Task<IActionResult> GetSubscription(string companyAccountId, CancellationToken cancellationToken)
    {
        var view = await _queries.Handle(new GetSubscriptionViewQuery(companyAccountId), cancellationToken);
        if (view == null)
        {
            return NotFound(new { message = "Company account was not found." });
        }

        return Ok(SubscriptionViewResourceFromQueryAssembler.ToResourceFromView(view));
    }

    [HttpPost("{companyAccountId}/subscription")]
    public async Task<IActionResult> SelectSubscriptionPlan(
        string companyAccountId,
        [FromBody] SelectSubscriptionPlanResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var command = SelectSubscriptionPlanCommandFromResourceAssembler.ToCommandFromResource(
                companyAccountId,
                resource);
            var subscription = await _commands.Handle(command, cancellationToken);
            return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
        }
        catch (CommercialRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPost("{companyAccountId}/subscription/activation")]
    public async Task<IActionResult> ActivateSubscription(
        string companyAccountId,
        CancellationToken cancellationToken)
    {
        try
        {
            var subscription = await _commands.Handle(
                new ActivateSubscriptionCommand(companyAccountId),
                cancellationToken);
            return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
        }
        catch (CommercialRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    [HttpPost("{companyAccountId}/subscription/renewal")]
    public async Task<IActionResult> RenewSubscription(
        string companyAccountId,
        CancellationToken cancellationToken)
    {
        try
        {
            var subscription = await _commands.Handle(
                new RenewSubscriptionCommand(companyAccountId),
                cancellationToken);
            return Ok(SubscriptionResourceFromEntityAssembler.ToResourceFromEntity(subscription));
        }
        catch (CommercialRuleException exception)
        {
            return MapRuleException(exception);
        }
    }

    private IActionResult MapRuleException(CommercialRuleException exception)
    {
        if (exception.NotFound)
        {
            return NotFound(new { message = exception.Message });
        }

        return BadRequest(new { message = exception.Message });
    }
}
