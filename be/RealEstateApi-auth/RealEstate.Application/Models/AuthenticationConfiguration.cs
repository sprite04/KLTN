﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models
{
    public class AuthenticationConfiguration
    {
        public string AccessTokenSecret { get; set; }
        public int AccessTokenExpirationMinutes { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string RefreshTokenSecret { get; set; }
        public int RefreshTokenExpirationMinutes { get; set; }
    }
}
