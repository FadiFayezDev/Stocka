using Application.Common.Interfaces;
using AutoMapper;

namespace Application.Bases
{
    public abstract class BaseHandler<T> : ResponseHandler where T : class
    {
        protected T _repo;  // Alias for compatibility
        protected readonly IMapper? _mapper;
        protected readonly IUnitOfWork? _work;

        public BaseHandler(IMapper? mapper, T repo, IUnitOfWork? work = null)
        {
            _repo = repo;
            _mapper = mapper;
            _work = work;
        }

        public BaseHandler(IMapper? mapper, T services) : this(mapper, services, null)
        {
        }

        /// <summary>
        /// Executes a create operation with proper transaction handling
        /// </summary>
        protected async Task<Response<TDto>> ExecuteCreateAsync<TEntity, TDto>(
            TEntity entity,
            Func<TEntity, Task<bool>> createAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TDto : class
        {
            try
            {
                await _work!.BeginTransactionAsync();

                var success = await createAction(entity);
                if (!success)
                    return new Response<TDto>("Failed to create resource");

                var result = await _work.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                    return new Response<TDto>("Failed to persist changes");

                await _work.CommitTransactionAsync();

                var dto = _mapper!.Map<TDto>(entity);
                return new Response<TDto>(dto, "Created Successfully");
            }
            catch (Exception)
            {
                await _work!.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Executes an update operation with proper transaction handling
        /// </summary>
        protected async Task<Response<TDto>> ExecuteUpdateAsync<TEntity, TDto>(
            TEntity entity,
            Func<TEntity, Task<bool>> updateAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TDto : class
        {
            try
            {
                await _work!.BeginTransactionAsync();

                var success = await updateAction(entity);
                if (!success)
                    return new Response<TDto>("Failed to update resource");

                var result = await _work.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                    return new Response<TDto>("Failed to persist changes");

                await _work.CommitTransactionAsync();

                var dto = _mapper!.Map<TDto>(entity);
                return new Response<TDto>(dto, "Updated Successfully");
            }
            catch (Exception)
            {
                await _work!.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Executes a delete operation with proper transaction handling
        /// </summary>
        protected async Task<Response<bool>> ExecuteDeleteAsync<TEntity>(
            TEntity entity,
            Func<TEntity, Task<bool>> deleteAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            try
            {
                await _work!.BeginTransactionAsync();

                var success = await deleteAction(entity);
                if (!success)
                    return new Response<bool>(false, "Failed to delete resource");

                var result = await _work.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                    return new Response<bool>(false, "Failed to persist changes");

                await _work.CommitTransactionAsync();

                return new Response<bool>(true, "Deleted Successfully");
            }
            catch (Exception)
            {
                await _work!.RollbackTransactionAsync();
                throw;
            }
        }
    }

    public abstract class BaseHandler<Command, Query> : ResponseHandler where Command : class where Query : class
    {
        protected readonly Query _query;
        protected readonly Command _command;
        protected readonly IMapper? _mapper;
        protected readonly IUnitOfWork? _work;

        public BaseHandler(IMapper mapper, Command command, Query query, IUnitOfWork work)
        {
            _mapper = mapper;
            _command = command;
            _query = query;
            _work = work;
        }

        public BaseHandler(Command command, Query query, IUnitOfWork work)
        {
            _command = command;
            _query = query;
            _work = work;
        }

        /// <summary>
        /// Executes a create operation with proper transaction handling
        /// </summary>
        protected async Task<Response<TDto>> ExecuteCreateAsync<TEntity, TDto>(
            TEntity entity,
            Func<TEntity, Task<bool>> createAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TDto : class
        {
            try
            {
                await _work!.BeginTransactionAsync();

                var success = await createAction(entity);
                if (!success)
                    return new Response<TDto>("Failed to create resource");

                var result = await _work.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                    return new Response<TDto>("Failed to persist changes");

                await _work.CommitTransactionAsync();

                var dto = _mapper!.Map<TDto>(entity);
                return new Response<TDto>(dto, "Created Successfully");
            }
            catch (Exception)
            {
                await _work!.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Executes an update operation with proper transaction handling
        /// </summary>
        protected async Task<Response<TDto>> ExecuteUpdateAsync<TEntity, TDto>(
            TEntity entity,
            Func<TEntity, Task<bool>> updateAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
            where TDto : class
        {
            try
            {
                await _work!.BeginTransactionAsync();

                var success = await updateAction(entity);
                if (!success)
                    return new Response<TDto>("Failed to update resource");

                var result = await _work.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                    return new Response<TDto>("Failed to persist changes");

                await _work.CommitTransactionAsync();

                var dto = _mapper!.Map<TDto>(entity);
                return new Response<TDto>(dto, "Updated Successfully");
            }
            catch (Exception)
            {
                await _work!.RollbackTransactionAsync();
                throw;
            }
        }

        /// <summary>
        /// Executes a delete operation with proper transaction handling
        /// </summary>
        protected async Task<Response<bool>> ExecuteDeleteAsync<TEntity>(
            TEntity entity,
            Func<TEntity, Task<bool>> deleteAction,
            CancellationToken cancellationToken = default)
            where TEntity : class
        {
            try
            {
                await _work!.BeginTransactionAsync();

                var success = await deleteAction(entity);
                if (!success)
                    return new Response<bool>(false, "Failed to delete resource");

                var result = await _work.SaveChangesAsync(cancellationToken);
                if (result <= 0)
                    return new Response<bool>(false, "Failed to persist changes");

                await _work.CommitTransactionAsync();

                return new Response<bool>(true, "Deleted Successfully");
            }
            catch (Exception)
            {
                await _work!.RollbackTransactionAsync();
                throw;
            }
        }
    }
}