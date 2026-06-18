using System;
using System.Collections.Generic;
using System.Text;

namespace GymManagementSystem.BLL.Common
{
    public sealed record Result(bool success, string? error = null, ResultKind Kind = ResultKind.OK)
    {
        public static Result Ok() => new(true, null, ResultKind.OK);
        public static Result Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, error, kind);
        public static Result NotFound(string error = "Resource not found.") => new(false, error, ResultKind.NotFound);
        public static Result Validation(string error) => new(false, error, ResultKind.ValidationFailed);
        public static Result BusinessRule(string error) => new(false, error, ResultKind.BusinessRule);
        public static Result Unauthorized(string error = "Unauthorized.") =>new(false, error, ResultKind.Unauthorized);
        public static Result Forbidden(string error = "Forbidden.") =>new(false, error, ResultKind.Forbidden);
    }
    public sealed record Result<T>(bool success, T? value, string? error = null, ResultKind kind = ResultKind.OK)
    {
        public static Result<T> OK(T value) => new(true, value);
        public static Result<T> Fail(string error, ResultKind kind = ResultKind.Conflict) => new(false, default, error, kind);
        public static Result<T> NotFound(string error = "NotFound") => new(false, default, error, ResultKind.NotFound);

    }
}
