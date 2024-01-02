using ChauffeurApp.Application.DTOs;
using ChauffeurApp.Application.Services.IServices;
using ChauffeurApp.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ChauffeurApp.Application.Helper;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Hosting;


namespace ChauffeurApp.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly IValidator<ApplicationUser> _validator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService, IConfiguration config, IValidator<ApplicationUser> validator, IHttpContextAccessor httpContextAccessor, IEmailService emailService, IHostingEnvironment hostingEnvironment)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _config = config;
            _validator = validator;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
            _hostingEnvironment = hostingEnvironment;

        }

        //Initiating phone number change
        public async Task<Result<bool>> InitiateChangePhoneNumber(ChangePhoneNumberDTO changePhoneNumberDTO)
        {
            var user = await _userManager.Users.Where(x => x.PhoneNumber == changePhoneNumberDTO.CurrentPhoneNumber).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<bool>.Failure("A user with this phone number does not exist");
            };

            var changePhoneNumberToken = await _userManager.GenerateChangePhoneNumberTokenAsync(user, changePhoneNumberDTO.UpdatedPhoneNumber);
            await _emailService.SendChangePhoneNumberTokenAsync(user.Email, changePhoneNumberToken);

            return Result<bool>.Success(true);
        }

        //After initiating phone number change, resetting the phone number
        public async Task<Result<bool>> ResetPhoneNumber(ResetPhoneNumberDTO resetPhoneNumberDTO)
        {
            var user = await _userManager.Users.Where(x => x.PhoneNumber == resetPhoneNumberDTO.CurrentPhoneNumber).FirstOrDefaultAsync();
            if (user == null)
            {
                return Result<bool>.Failure("A user with this email does not exist");
            };

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, resetPhoneNumberDTO.Token, resetPhoneNumberDTO.UpdatedPhoneNumber);
            if (!result)
            {
                return Result<bool>.Failure("Error in verification");
            }

            var changePhoneResult = await _userManager.ChangePhoneNumberAsync(user, resetPhoneNumberDTO.UpdatedPhoneNumber, resetPhoneNumberDTO.Token);
            if (!changePhoneResult.Succeeded)
            {
                return Result<bool>.Failure("Error");
            }

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Result<bool>.Failure("Error in updating user");
            }

            return Result<bool>.Success(true);

        }


        //User login
        public async Task<Result<LoginResponseDTO>> Login(LoginDTO loginDTO)
        {
            var identityUser = await _userManager.Users.Where(x => x.PhoneNumber == loginDTO.PhoneNumber).FirstOrDefaultAsync();

            if (identityUser == null)
            {
                return Result<LoginResponseDTO>.Failure("A user with this phone number does not exist");
            };

            var response = await _userManager.CheckPasswordAsync(identityUser, loginDTO.Password);
            var twoFactorToken = await _userManager.GenerateTwoFactorTokenAsync(identityUser, "Email");

            if (response)
            {
                var roles = await _userManager.GetRolesAsync(identityUser);
                var token = await _tokenService.CreateToken(identityUser, roles);
                var newRefreshToken = await _tokenService.CreateNewRefreshToken(identityUser.Id.ToString(), Guid.NewGuid().ToString());

                LoginResponseDTO responseDto = new LoginResponseDTO()
                {
                    FullName = identityUser.FullName,
                    Email = identityUser.Email,
                    Token = token.Value,
                    RefreshToken = newRefreshToken.Value
                };
                return Result<LoginResponseDTO>.Success(responseDto);
            }
            else
            {
                return Result<LoginResponseDTO>.Failure("Wrong password for this user");
            }
        }

        //private void SendTokenToEmail(string emailAddress, string token)
        //{
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUsername").Value));
        //    email.To.Add(MailboxAddress.Parse(emailAddress));
        //    email.Subject = "Token to confirm change in phone number";
        //    email.Body = new TextPart(TextFormat.Html) { Text = token };

        //    using var smtp = new SmtpClient();
        //    smtp.Connect(_config.GetSection("EmailHost").Value, 587, SecureSocketOptions.StartTls);
        //    smtp.Authenticate(_config.GetSection("EmailUsername").Value, _config.GetSection("EmailPassword").Value);
        //    smtp.Send(email);
        //    smtp.Disconnect(true);
        //}


        //User registration
        public async Task<Result<bool>> RegisterUser(CreateUserDTO createUserDTO)
        {
            var email = await _userManager.FindByEmailAsync(createUserDTO.Email);
            if (email != null)
            {
                return Result<bool>.Failure("This email already exists!");
            }

            var phoneNumber = await _userManager.Users.Where(x => x.PhoneNumber == createUserDTO.PhoneNumber).FirstOrDefaultAsync();
            if (phoneNumber != null)
            {
                return Result<bool>.Failure("A user with this phone number already exists");
            };

            var identityUser = new ApplicationUser()
            {
                FullName = createUserDTO.FullName,
                PhoneNumber = createUserDTO.PhoneNumber,
                Gender = createUserDTO.Gender,
                UserName = createUserDTO.Email,
                Email = createUserDTO.Email,
                TwoFactorEnabled = true,
                
            };



            var validationResult = await _validator.ValidateAsync(identityUser);

            if (!validationResult.IsValid)
            {
                string errors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => e.ErrorMessage));
                return Result<bool>.Failure(errors);
            }

            var result = await _userManager.CreateAsync(identityUser, createUserDTO.Password);
            if (!result.Succeeded)
            {
                return Result<bool>.Failure("Error in creating the user");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);

            var confirmationLink = GenerateConfirmationLink(identityUser.Id, token);


            //var confirmationLink = $"https://localhost:7202/api/EmailConfirmation/verify-email?userId={identityUser.Id}&token={token}";

            var emailSubject = "Confirm your email address";
            var emailMessage = $"Please click <a href=\"{confirmationLink}\">here</a> to confirm your email address.";

            
            EmailDTO emailDTO = new EmailDTO()
            {
                To = identityUser.Email,
                Body = emailMessage,
                Subject = emailSubject,
            };
            _emailService.SendEmail(emailDTO);

            var role = await _userManager.AddToRoleAsync(identityUser, createUserDTO.Role.ToString());

            if (!role.Succeeded)
            {
                return Result<bool>.Failure("Error in assigning role to user");
            }
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> ConfirmPhoneNumber(string userId, string phoneNumber, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.Failure("User not found");
            }

            var result = await _userManager.VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber);
            if (!result)
            {
                return Result<bool>.Failure("Invalid token or phone number");
            }

            user.PhoneNumberConfirmed = true;
            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                return Result<bool>.Failure("Error confirming phone number");
            }

            return Result<bool>.Success(true);
        }

        private string GenerateConfirmationLink(long userId, string token)
        {
            string userIdString = userId.ToString();
            string baseUrl = GetBaseUrl();
            return $"{baseUrl}/api/EmailConfirmation/verify-email?userId={userId}&token={token}";
        }

        private string GetBaseUrl()
        {
            string baseUrl;

            if (_hostingEnvironment.IsDevelopment())
            {
                //var j = _hostingEnvironment.get
                baseUrl = "https://localhost:7202";
            }
            else
            {
                baseUrl = "https://your_production_domain";
            }

            return baseUrl;
        }
        //private async Task SendConfirmationEmail(string emailAddress, string subject, string htmlMessage)
        //{
        //    if (string.IsNullOrEmpty(emailAddress))
        //    {
        //        // Log or throw an exception indicating that the email address is null or empty.
        //        // Handle this case according to your application's logic.
        //        throw new ArgumentNullException(nameof(emailAddress), "Email address is null or empty.");
        //    }
        //    var emailMessage = new MimeMessage();
        //    emailMessage.From.Add(MailboxAddress.Parse(_config["EmailSettings:EmailUsername"]));
        //    emailMessage.To.Add(MailboxAddress.Parse(emailAddress));
        //    emailMessage.Subject = subject;
        //    emailMessage.Body = new TextPart(TextFormat.Html) { Text = htmlMessage };

        //    using var smtpClient = new SmtpClient();
        //    smtpClient.Connect(_config["EmailSettings:EmailHost"], 587, SecureSocketOptions.StartTls);
        //    smtpClient.Authenticate(_config["EmailSettings:EmailUsername"], _config["EmailSettings:EmailPassword"]);
        //    await smtpClient.SendAsync(emailMessage);
        //    await smtpClient.DisconnectAsync(true);
        //}


        public async Task<Result<bool>> VerifyEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return Result<bool>.Failure("User not found");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (!result.Succeeded)
            {
                return Result<bool>.Failure("Error confirming email");
            }

            return Result<bool>.Success(true);
        }

        //Upload profile picture
        public async Task<Result<string>> UploadFile(IFormFile _IFormFile)
        {
            string email = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(email))
            {
                return Result<string>.Failure("User email not found.");
            }

            var user = await _userManager.Users.Where(x => x.Email == email).FirstOrDefaultAsync();

            if (user == null)
            {
                return Result<string>.Failure("User not found.");
            }

            if (_IFormFile == null || _IFormFile.Length == 0)
            {
                return Result<string>.Failure("File is empty or not provided.");
            }

           
            long fileSizeLimit = 5 * 1024 * 1024; // 5 MB
            if (_IFormFile.Length > fileSizeLimit)
            {
                return Result<string>.Failure($"File size exceeds the limit of {fileSizeLimit / (1024 * 1024)} MB.");
            }

            
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" }; 
            var fileExtension = Path.GetExtension(_IFormFile.FileName).ToLowerInvariant();
            if (!allowedExtensions.Contains(fileExtension))
            {
                return Result<string>.Failure("Invalid file format. Allowed formats: " + string.Join(", ", allowedExtensions));
            }

            string fileName = "";
            try
            {
                FileInfo fileInfo = new FileInfo(_IFormFile.FileName);
                fileName = $"{Guid.NewGuid().ToString()}{fileExtension}";
                var filePath = Common.GetFilePath(fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await _IFormFile.CopyToAsync(fileStream);
                }

                user.ProfilePicturePath = fileName;

                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    return Result<string>.Failure("Error updating user profile.");
                }

                return Result<string>.Success(fileName);
            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
                return Result<string>.Failure("An error occurred while uploading the file.");
            }

        }
    }
}
