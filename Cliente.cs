using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BillingEnd.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Facturas = new HashSet<Factura>();
        }
        [Display(Name = "NOMBRE CLIENTE")]
        public string CliNombre {get;set;}
        [Display(Name = "ID CLIENTE")]

        public int CliId { get; set; }
        [Display(Name = "NUMERO TELEFONICO")]

        public string CliNum { get; set; }
        [Display(Name = "DIRECCION")]

        public string CliDireccion { get; set; }
        [Display(Name = "DNI")]

        public string CliDni { get; set; }
        [Display(Name = "CUIL")]

        public string CliCuil { get; set; }

        public virtual ICollection<Factura> Facturas { get; set; }
    }
}
