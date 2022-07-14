using RealEstate.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Application.Models
{
    public class ResponseUser: Response
    {
        public UserDto User { get; set; }
    }
}
