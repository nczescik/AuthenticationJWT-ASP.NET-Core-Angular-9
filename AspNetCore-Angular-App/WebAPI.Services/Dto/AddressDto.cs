using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.Services.Dto
{
    public class AddressDto
    {
        public long AddressId { get; set; }
        public string Street { get; set; }
        public string Postcode { get; set; }
    }
}
