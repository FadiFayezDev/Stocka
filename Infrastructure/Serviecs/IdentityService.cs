using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Dtos.Auth;
using Infrastructure.Exceptions;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Serviecs
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        #region User Section

        public async Task<UserRegistrationResponseDto> CreateUserAsync(CreateUserDto userDto)
        {
            var user = new ApplicationUser()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                UserName = userDto.UserName,
                Email = userDto.Email,
            };

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (!result.Succeeded)
                throw new IdentityOperationException(result.Errors);

            if (userDto.Roles != null && userDto.Roles.Any())
            {
                var addUserRole = await _userManager.AddToRolesAsync(user, userDto.Roles);
                if (!addUserRole.Succeeded)
                    throw new IdentityOperationException(addUserRole.Errors);
            }

            return new UserRegistrationResponseDto(result.Succeeded, user.Id);
        }

        public async Task<bool> SigninUserAsync(string userName, string password)
        {
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded;
        }

        public async Task<Guid> GetUserIdAsync(string userName)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == userName);
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            return user.Id;
        }

        public async Task<UserDetailsDto> GetUserDetailsAsync(Guid userId)
        {
            var user = await _userManager.Users
                .Include(u => u.BrandUsers)
                .FirstOrDefaultAsync(x => x.Id == userId);

            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            return await MapToUserDetailsDto(user);
        }

        public async Task<UserDetailsDto> GetUserDetailsByUserNameAsync(string userName)
        {
            var user = await _userManager.Users
                .Include(u => u.BrandUsers)
                .FirstOrDefaultAsync(x => x.UserName == userName);

            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            return await MapToUserDetailsDto(user);
        }

        public async Task<string> GetUserNameAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");
            return user.UserName;
        }

        public async Task<bool> DeleteUserAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            if (user.UserName == "system" || user.UserName == "admin")
                throw new BadRequestException("You can not delete system or admin user");

            var result = await _userManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> IsUniqueUserName(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            return user == null;
        }

        public async Task<List<UserSummaryDto>> GetAllUsersAsync()
        {
            return await _userManager.Users
                .Select(user => new UserSummaryDto(
                    user.Id,
                    user.FirstName,
                    user.LastName,
                    user.UserName,
                    user.Email))
                .ToListAsync();
        }

        public async Task<List<UserDetailsDto>> GetAllUsersDetailsAsync()
        {
            var users = await _userManager.Users.Include(u => u.BrandUsers).ToListAsync();
            var userDetailsList = new List<UserDetailsDto>();

            foreach (var user in users)
            {
                userDetailsList.Add(await MapToUserDetailsDto(user));
            }

            return userDetailsList;
        }

        public async Task<bool> UpdateUserProfile(UpdateUserProfileDto profileDto)
        {
            var user = await _userManager.FindByIdAsync(profileDto.Id);
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            user.FirstName = profileDto.FirstName;
            user.LastName = profileDto.LastName;
            user.Email = profileDto.Email;

            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        #endregion

        #region Role Section

        public async Task<bool> CreateRoleAsync(string roleName)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            if (!result.Succeeded) throw new IdentityOperationException(result.Errors);
            return result.Succeeded;
        }

        public async Task<bool> DeleteRoleAsync(Guid roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) throw new NotFoundException("ROLE_NOT_FOUND");

            if (role.Name == "Administrator")
                throw new BadRequestException("You cannot delete Administrator Role");

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded) throw new IdentityOperationException(result.Errors);

            return result.Succeeded;
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            return await _roleManager.Roles
                .Select(role => new RoleDto(role.Id, role.Name))
                .ToListAsync();
        }

        public async Task<RoleDto> GetRoleByIdAsync(Guid id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());
            if (role == null) throw new NotFoundException("ROLE_NOT_FOUND");
            return new RoleDto(role.Id, role.Name);
        }

        public async Task<bool> UpdateRole(RoleDto roleDto)
        {
            var role = await _roleManager.FindByIdAsync(roleDto.Id.ToString());
            if (role == null) throw new NotFoundException("ROLE_NOT_FOUND");

            role.Name = roleDto.RoleName;
            var result = await _roleManager.UpdateAsync(role);
            return result.Succeeded;
        }

        #endregion

        #region User's Role Section

        public async Task<bool> IsInRoleAsync(Guid userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<List<string>> GetUserRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<bool> AssignUserToRole(string userName, IList<string> roles)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            var result = await _userManager.AddToRolesAsync(user, roles);
            return result.Succeeded;
        }

        public async Task<bool> UpdateUsersRole(string userName, IList<string> usersRole)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null) throw new NotFoundException("USER_NOT_FOUND");

            var existingRoles = await _userManager.GetRolesAsync(user);
            var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);

            if (!removeResult.Succeeded) return false;

            var addResult = await _userManager.AddToRolesAsync(user, usersRole);
            return addResult.Succeeded;
        }

        #endregion

        #region Private Helpers

        private async Task<UserDetailsDto> MapToUserDetailsDto(ApplicationUser user)
        {
            var brandIds = user.BrandUsers?.Select(bu => bu.BrandId).ToList() ?? new List<Guid>();
            var roles = await _userManager.GetRolesAsync(user);

            return new UserDetailsDto(
                user.Id,
                brandIds,
                user.FirstName,
                user.LastName,
                user.UserName,
                user.Email,
                roles
            );
        }

        #endregion
    }
}