using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Bases
{
    public class RepoResult<T> where T : class
    {
        public RepoResultStatus Status { get; set; }
        public T? Data { get; set; }
        public string? Error { get; set; }

        private RepoResult(RepoResultStatus status, T? data = default, string? error = null)
        {
            Status = status;
            Data = data;
            Error = error;
        }

        private RepoResult(RepoResultStatus status, string? error = null)
        {
            Status = status;
            Error = error;
        }

        private RepoResult(RepoResultStatus status)
        {
            Status = status;
        }

        public RepoResult()
        {
        }

        public static RepoResult<T> SuccessResult(T data)
            => new(RepoResultStatus.Success, data);

        public static RepoResult<T> NotFoundResult(string error)
            => new(RepoResultStatus.NotFound, default, error);

        public static RepoResult<T> ErrorResult(string error)
            => new(RepoResultStatus.Error, default, error);

        public static RepoResult<T> CreatedResult(T data)
            => new(RepoResultStatus.Created, data);

        public static RepoResult<T> UpdatedResult(T data)
            => new(RepoResultStatus.Updated, data);

        public static RepoResult<T> DeletedResult()
            => new(RepoResultStatus.Deleted);

        public static RepoResult<T> FailureResult(string error)
            => new(RepoResultStatus.Failure, error);
    }

    public enum RepoResultStatus
    {
        Success,
        NotFound,
        Error,
        Created,
        Updated,
        Deleted,
        ModelIsAlreadyExist,
        Failure
    }
}