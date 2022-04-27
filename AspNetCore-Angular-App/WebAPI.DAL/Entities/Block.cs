using System;
using System.Collections.Generic;
using System.Text;

namespace WebAPI.DAL.Entities
{
    public class Block : Entity
    {
        public int Number { get; set; }

        public virtual Address Address { get; set; }
    }
}
