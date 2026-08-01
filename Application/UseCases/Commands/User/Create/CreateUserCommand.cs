using Application.Common.Interfaces;
using Application.DTOs;
using MediatR;

namespace Application.UseCases.Commands.User.Create
{
    public class CreateUserCommand : IRequest<AuthResponseDTO>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public List<string> Roles { get; set; }
    }

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, AuthResponseDTO>
    {
        private readonly IIdentityService _identityService;
        //private readonly IPersonCommandRepository _personCommand;
        //private readonly ISocialMediaCommandRepository _socialMediaCommand;
        private readonly IUnitOfWork _work;
        private readonly ITokenGenerator _tokenGenerator;

        public CreateUserCommandHandler(IIdentityService identityService, ITokenGenerator tokenGenerator, IUnitOfWork work)
        {
            _identityService = identityService;
            _tokenGenerator = tokenGenerator;
            //this._personCommand = personCommand;
            //this._socialMediaCommand = socialMediaCommand;
            this._work = work;
        }
        public async Task<AuthResponseDTO> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            //await _work.BeginTransactionAsync();
            //try
            //{
            //    ////var person = await _personCommand.CreateAsync(new Person { FirstName = request.FirstName, LastName = request.LastName, DateOfBirth = request.DateOfBirth, Gender = (GenderType)request.Gender });

            //    ////await _socialMediaCommand.CreateAsync(new SocialMedia { PersonId = person.Id, MediaValue = request.Email, MediaType = SocialMediaType.Email });
            //    ////await _work.SaveChangesAsync();

            //    //var user = new CreateUserDto(
            //    //    request.FirstName, 
            //    //    request.LastName, 
            //    //    request.UserName, 
            //    //    request.Password, 
            //    //    request.Email, 
            //    //    request.Roles
            //    //);

            //    //var userRegistration = await _identityService.CreateUserAsync(user);
            //    //            await _work.CommitTransactionAsync();

            //    //var userToken = new UserTokenDetailsDto
            //    //(
            //    //    userRegistration.UserId,

            //    //    request.UserName,
            //    //    request.Roles
            //    //);

            //    //var token = _tokenGenerator.GenerateJWTToken((userRegistration.UserId, request.UserName, request.Roles));

            //    //return new AuthResponseDTO { UserId = userId, Name = request.UserName, Token = token };
            //}
            //catch
            //{
            //    _work.RollbackTransaction();
            //    throw;
            //}

            throw new NotImplementedException();
        }
    }
}