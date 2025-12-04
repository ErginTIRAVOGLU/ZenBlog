using System;

namespace ZenBlog.Application.Concrete;

public sealed class Result<T>
{
    public T Data { get; }
    public bool IsSuccess => !Errors.Any();
    public IReadOnlyCollection<Error> Errors { get; }

    private Result(T data, IReadOnlyCollection<Error> errors)
    {
        Data = data;
        Errors = errors;
    }

    public static Result<T> Success(T data) => new Result<T>(data, Array.Empty<Error>());

    public static Result<T> Failure(params Error[] errors)
    {
        if (errors == null || errors.Length == 0)
            throw new ArgumentException("At least one error is required for a failed result.", nameof(errors));
        
        return new Result<T>(default!, errors);
    }

    public static Result<T> Failure(IEnumerable<Error> errors)
    {
        var errorList = errors?.ToList() ?? new List<Error>();
        if (errorList.Count == 0)
            throw new ArgumentException("At least one error is required for a failed result.", nameof(errors));
        
        return new Result<T>(default!, errorList);
    }

    public static implicit operator bool(Result<T> result) => result.IsSuccess;
}

public sealed class Error
{
    public string PropertyName { get; }
    public string ErrorMessage { get; }

    public Error(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }
}