using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace BillingEnd.Models
{
    public partial class Medidas
    {
        public int ID { get; set; }

        public decimal Medida {get;set;}
       
    }
}
