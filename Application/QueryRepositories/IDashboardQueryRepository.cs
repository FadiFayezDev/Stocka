using Application.Dtos;

namespace Application.QueryRepositories
{
    public interface IDashboardQueryRepository
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync(Guid brandId);
    }
}