using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RealEstate.Application.DTOs;
using RealEstate.Application.Models;
using RealEstate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Tokens
{
    public class AccessTokenGenerator
    {
        private readonly AuthenticationConfiguration _configuration;
        private readonly TokenGenerator _tokenGenerator;
        public AccessTokenGenerator(AuthenticationConfiguration configuration,TokenGenerator tokenGenerator)
        {
            _configuration = configuration;
            _tokenGenerator = tokenGenerator;
        }
        public string GenerateToken(UserDto user)
        {
            IdentityOptions _option = new IdentityOptions();
            var claims = new[]
                {
                    new Claim("id",user.Id),
                    new Claim("username",user.UserName),
                    new Claim("name",user.Name),
                    new Claim("rolename",user.RoleName??"empty"),
                    new Claim("roleid",user.RoleID??"empty"),
                    new Claim("avatar",user.ImageUrl??"empty"),
                    new Claim(_option.ClaimsIdentity.RoleClaimType,user.RoleName??"User")
                };

            return _tokenGenerator.GenerateToken(
                _configuration.AccessTokenSecret, 
                _configuration.Issuer, 
                _configuration.Audience, 
                _configuration.AccessTokenExpirationMinutes, 
                claims);
        }
    }
}
