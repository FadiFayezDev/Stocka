using Identity.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Identity.Application.Common.Interfaces
{
    /// <summary>
    /// Contract for the module's ASP.NET Identity service (users / roles / memberships / auth).
    /// <para>
    /// Implemented by <c>Identity.Infrastructure.Services.IdentityService</c> on top of
    /// UserManager/RoleManager. Person data (names/phone) lives on the linked Party row;
    /// identity fields (username/email) live on the ApplicationUser row.
    /// </para>
    /// </summary>
    public interface IIdentityService
    {
        #region Authentication (JWT issuance)

        /// <summary>Self-service registration; the organization comes from the caller (e.g. tenant context).</summary>
        Task<AuthModel> RegisterAsync(RegisterModel model, Guid organizationId);

        Task<AuthModel> LoginAsync(LoginModel model);

        /// <summary>MVC variant of <see cref="LoginAsync"/> — the response additionally carries the UserId.</summary>
        Task<AuthModelMVC> LoginMvcAsync(TokenRequestModel model);

        #endregion

        #region Users

        /// <summary>Creates the user together with its linked person Party (atomically, one save).</summary>
        Task<UserRegistrationResponseDto> CreateUserAsync(CreateUserDto userDto);

        Task<bool> SignInUserAsync(string userName, string password);

        Task<Guid> GetUserIdAsync(string userName);

        Task<UserDetailsDto> GetUserDetailsAsync(Guid userId);

        Task<UserDetailsDto> GetUserDetailsByUserNameAsync(string userName);

        Task<string> GetUserNameAsync(Guid userId);

        Task<bool> DeleteUserAsync(Guid userId);

        Task<bool> IsUniqueUserNameAsync(string userName);

        Task<List<UserSummaryDto>> GetAllUsersAsync();

        Task<List<UserDetailsDto>> GetAllUsersDetailsAsync();

        /// <summary>Names persist on the Party, the email on the user row.</summary>
        Task<bool> UpdateUserProfile(UpdateUserProfileDto profileDto);

        /// <summary>The organization membership context of the user's Party (replaces the legacy brand memberships).</summary>
        Task<IReadOnlyList<Guid>> GetUserOrganizationIdsAsync(Guid userId);

        #endregion

        #region Roles

        Task<bool> CreateRoleAsync(string roleName);

        Task<bool> DeleteRoleAsync(Guid roleId);

        Task<List<RoleDto>> GetRolesAsync();

        Task<RoleDto> GetRoleByIdAsync(Guid id);

        Task<RoleDto> GetRoleByNameAsync(string name);

        Task<bool> RoleIsExistAsync(string name);

        Task<bool> UpdateRole(RoleDto roleDto);

        #endregion

        #region Users' Roles

        Task<bool> IsInRoleAsync(Guid userId, string role);

        Task<List<string>> GetUserRolesAsync(Guid userId);

        Task<bool> AssignUserToRole(string userName, IList<string> roles);

        Task<bool> UpdateUsersRole(string userName, IList<string> usersRole);

        #endregion
    }
}
