using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.DAL.Entities
{
    public class Address : Entity
    {
        public string Street { get; set; }
        public string Postcode { get; set; }

        public virtual User User { get; set; }

        public virtual List<Block> Blocks { get; set; }
    }
}
