﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models.Request
{
    public class RefreshRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
