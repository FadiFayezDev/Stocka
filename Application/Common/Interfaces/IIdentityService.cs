using Application.Dtos.Auth;
using Application.Dtos.Orders;
using Domain.Enums;

namespace Application.Common.Interfaces
{
    public interface IIdentityService
    {
        #region User Section
        Task<UserRegistrationResponseDto> CreateUserAsync(CreateUserDto userDto);
        Task<bool> SigninUserAsync(string userName, string password);
        Task<Guid> GetUserIdAsync(string userName);
        Task<UserDetailsDto> GetUserDetailsAsync(Guid userId);
        Task<UserDetailsDto> GetUserDetailsByUserNameAsync(string userName);
        Task<string> GetUserNameAsync(Guid userId);
        Task<bool> DeleteUserAsync(Guid userId);
        Task<bool> IsUniqueUserName(string userName);
        Task<List<UserSummaryDto>> GetAllUsersAsync();
        Task<List<UserDetailsDto>> GetAllUsersDetailsAsync();
        Task<bool> UpdateUserProfile(UpdateUserProfileDto profileDto);
        #endregion

        #region Role Section
        Task<bool> CreateRoleAsync(string roleName);
        Task<bool> DeleteRoleAsync(Guid roleId);
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(Guid id);
        Task<RoleDto> GetRoleByNameAsync(string name);
        Task<bool> RoleIsExistAsync(string roleName);
        Task<bool> UpdateRole(RoleDto roleDto);
        #endregion

        #region User's Role Section
        Task<bool> IsInRoleAsync(Guid userId, string role);
        Task<List<string>> GetUserRolesAsync(Guid userId);
        Task<bool> AssignUserToRole(string userName, IList<string> roles);
        Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);
        #endregion

        #region Brand Membership Section
        Task<List<BrandMembershipContextDto>> GetBrandMembershipsAsync(Guid userId);
        Task<BrandRole?> GetBrandRoleAsync(Guid userId, Guid brandId);
        Task<Guid?> GetEmployeeBranchIdAsync(Guid userId, Guid brandId);
        #endregion
    }
}
