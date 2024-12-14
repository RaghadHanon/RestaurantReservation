using RestaurantReservation.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Api.Contracts.Models.Authentication
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = ApiErrors.UserNameRequired)]
        public string Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = ApiErrors.EmailRequired)]
        public string Email { get; set; }

        [Required(ErrorMessage = ApiErrors.PasswordRequired)]
        public string Password { get; set; }

    }
}