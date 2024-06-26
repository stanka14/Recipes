﻿using System.ComponentModel.DataAnnotations;

namespace Dtos.Dtos
{
    public class UpdateUserDto
    {
        public UpdateUserDto() { }
        public string Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        public string Firstname { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        public string Lastname { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters long.")]
        public string Username { get; set; }
    }
}
