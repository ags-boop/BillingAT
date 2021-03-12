using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
#nullable disable

namespace BillingEnd.Models
{
    public partial class Etiqueta
    {
        public int ID { get; set; }

        public string Etiquetas {get;set;}
       
    }
}
