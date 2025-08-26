using FCamara.Commission.Application.Shared;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FCamara.Commission.Api.Extensions;

public static class ResultExtensions
{
    public static IResult ToHttpResult<T>(
        this Result<T> result,
        int successStatusCode = StatusCodes.Status200OK)
    {
        if (result.IsSuccess)
        {
            return successStatusCode switch
            {
                StatusCodes.Status200OK => Results.Ok(result.Value),
                StatusCodes.Status202Accepted => Results.Accepted(),
                StatusCodes.Status201Created => Results.Created(string.Empty, result.Value),
                _ => Results.StatusCode(successStatusCode)
            };
        }

        return result.Error switch
        {
            ValidationException validationEx => ReturnHttpResult(validationEx),
            _ => Results.Problem(title: "Unexpected error", detail: result.Error?.ToString())
        };
    }
    
    private static IResult ReturnHttpResult(ValidationException validationEx)
    {
        var pd = new ValidationProblemDetails();

        foreach (var error in validationEx.Errors)
        {
            pd.Errors.TryAdd(error.PropertyName, [error.ErrorMessage]);
        }

        pd.Title = "Validation failed.";
        pd.Status = StatusCodes.Status400BadRequest;
        pd.Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1";

        return Results.Problem(
            title: pd.Title,
            statusCode: pd.Status,
            type: pd.Type,
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = pd.Errors
            }
        );
    }
}