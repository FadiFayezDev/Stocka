// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Application.Common.Exceptions;
using Application.UseCases.Brand;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace MVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class CreateBrandModel : PageModel
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CreateBrandModel> _logger;

        public CreateBrandModel(IMediator mediator, ILogger<CreateBrandModel> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        [BindProperty]
        public Guid? UserId { get; set; }

        [BindProperty]
        public string ReturnUrl { get; set; }

        public void OnGet(string userId = null, string returnUrl = null)
        {
            Input = new InputModel();
            if (Guid.TryParse(userId, out var parsed))
                UserId = parsed;
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (UserId is null || UserId == Guid.Empty)
            {
                ModelState.AddModelError(string.Empty, "Invalid user.");
                return Page();
            }

            try
            {
                var response = await _mediator.Send(new CreateBrandCommand
                {
                    Name = Input.BrandName.Trim(),
                    Slug = string.IsNullOrWhiteSpace(Input.Slug) ? GenerateSlug(Input.BrandName) : Input.Slug.Trim(),
                    UserId = UserId
                });

                Response.Cookies.Append("access_token", response.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Lax,
                    Expires = DateTimeOffset.UtcNow.AddMinutes(60)
                });

                return LocalRedirect(ReturnUrl ?? "~/");
            }
            catch (BusinessException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Brand creation failed for user {UserId}", UserId);
                ModelState.AddModelError(string.Empty, "An error occurred while creating the brand. Please try again.");
                return Page();
            }
        }

        private static string GenerateSlug(string name) =>
            name.Trim().ToLowerInvariant().Replace(' ', '-');
    }

    public class InputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at most {1} characters long.", MinimumLength = 3)]
        [Display(Name = "Brand Name")]
        public string BrandName { get; set; }

        [StringLength(100, ErrorMessage = "The {0} must be at most {1} characters long.")]
        [RegularExpression(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", ErrorMessage = "Slug can only contain lowercase letters, numbers and dashes.")]
        [Display(Name = "Slug")]
        public string Slug { get; set; }
    }
}
