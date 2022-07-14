﻿using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Tokens.TokenValidators
{
    public class RefreshTokenValidator
    {
        private readonly AuthenticationConfiguration _configuration;
        public RefreshTokenValidator(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }
        public bool Validate(string refreshToken)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidIssuer = _configuration.Issuer,
                ValidAudience = _configuration.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.RefreshTokenSecret)),
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(refreshToken,validationParameters , out SecurityToken validatedToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            
        }
    }
}
