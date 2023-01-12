using System.Collections.Generic;

namespace Kontatus.Helper.Utilitarios
{
    public enum ErrorType
    {
        Authorization = 1,
        Validation,
        Information,
        Other,
    }
    public class Result<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public ErrorType? Type { get; set; }
        public IEnumerable<string> Errors { get; set; }

        public static Result<T> Ok(T data)
        {
            return new Result<T> { Success = true, Data = data, };
        }

        public static Result<T> Ok()
        {
            return new Result<T> { Success = true };
        }

        public static Result<T> Err(IEnumerable<string> errors, ErrorType type = ErrorType.Other)
        {
            return new Result<T> { Success = false, Errors = errors, Type = type };
        }

        public static Result<T> Err(string error, ErrorType type = ErrorType.Other)
        {
            return new Result<T> { Success = false, Errors = new[] { error }, Type = type };
        }
    }
}
