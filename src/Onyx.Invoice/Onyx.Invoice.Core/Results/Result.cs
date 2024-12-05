using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace Onyx.Invoice.Core.Results;

public class Result<T>
{
    public Result(T data)
    {
        Success = true;
        Data = data;
        Errors = null!;
    }

    public Result(string error)
    {
        Success = false;
        Errors.Add(error);
    }

    public Result(List<string> errors)
    {
        Success = false;
        Errors = errors;
    }

    public Result(ValidationResult validationResult)
    {
        Success = false;
        Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
    }

    [JsonPropertyName("success")]
    public bool Success { get; private set; }

    [JsonPropertyName("result")]
    public T? Data { get; private set; }

    [JsonPropertyName("errors")]
    public List<string> Errors { get; } = [];
}