using System.Text.Json.Serialization;
using Enplt.Services.Api.Domain;
using Enplt.Services.Api.SaleManagerAvailability;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Enplt.Services.Api.Mappings;

public static class CalendarQueryPostMapping
{
    public static async Task<IResult> ExecuteAsync(
        HttpContext context,
        [FromBody] CalendarQueryRequest request,
        SaleManagerAvailabilityQuery query
    )
    {
        CalendarQueryRequestValidator validator = new ();
        ValidationResult validationResult = await validator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            return Results.BadRequest(validationResult.Errors);
        }

        List<Availability> managerAvailabilities =
            await query.ExecuteAsync(
                request.Date ?? throw new InvalidOperationException("Date must be specified"),
                request.Products ?? throw new InvalidOperationException("Products must be specified"),
                request.Language ?? throw new InvalidOperationException("Language must be specified"),
                request.Rating ?? throw new InvalidOperationException("Rating must be specified")
            );

        return Results.Ok(
            managerAvailabilities.Select(ConvertToCalendarQueryResponse)
        );
    }

    private static CalendarQueryResponse ConvertToCalendarQueryResponse(this Availability availability) =>
        new CalendarQueryResponse
        {
            AvailableCount = availability.AvailableCount,
            StartDate = availability.StartDate.ToUniversalTime()
        };
}


public sealed class CalendarQueryRequest
{
    [JsonPropertyName("date")]
    public DateOnly? Date { get; init; }

    [JsonPropertyName("products")]
    public List<Products>? Products { get; init; }

    [JsonPropertyName("language")]
    public SpokenLanguages? Language { get; init; }

    [JsonPropertyName("rating")]
    public CustomerRating? Rating { get; init; }
}

public sealed class CalendarQueryRequestValidator : AbstractValidator<CalendarQueryRequest>
{
    public CalendarQueryRequestValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(request => request.Date)
            .NotNull().WithMessage("Date must be specified");

        RuleFor(request => request.Products)
            .Must(products => products?.Count > 0).WithMessage("Products must be specified")
            .Must(products => products?.All(Enum.IsDefined) == true)
            .WithMessage($"Product times must be valid values")
            .Must(products => products?.Distinct().Count() == products?.Count).WithMessage("Products must be unique");

        RuleFor(request => request.Language)
            .NotNull().WithMessage("Language must be specified")
            .IsInEnum().WithMessage("Language must be a valid value");

        RuleFor(request => request.Rating)
            .NotNull().WithMessage("Rating must be specified")
            .IsInEnum().WithMessage("Rating must be a valid value");
    }
}

public sealed class CalendarQueryResponse
{
    [JsonPropertyName("available_count")]
    public required int AvailableCount { get; init; }

    [JsonPropertyName("start_date")]
    public required DateTimeOffset StartDate { get; init; }
}
