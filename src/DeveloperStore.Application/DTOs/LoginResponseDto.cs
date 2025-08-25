﻿namespace DeveloperStore.Application.DTOs
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public UserDto User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}