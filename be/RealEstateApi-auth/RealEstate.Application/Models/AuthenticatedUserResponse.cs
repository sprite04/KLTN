﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models
{
    public class AuthenticatedUserResponse
    {
        public AuthenticatedUserResponse(){}

        public string RefreshToken { get; set; }
        public string AccessToken { get; set; }
    }
}
