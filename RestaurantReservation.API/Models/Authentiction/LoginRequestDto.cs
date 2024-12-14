using RestaurantReservation.API.Utilities;
using System.ComponentModel.DataAnnotations;

namespace RestaurantReservation.Api.Contracts.Models.Authentication;

public class LoginRequestDto
{
    [Required(ErrorMessage = ApiErrors.UserNameRequired )]
    public string Username { get; set; }

    [Required(ErrorMessage = ApiErrors.PasswordRequired )]
    public string Password { get; set; }
}