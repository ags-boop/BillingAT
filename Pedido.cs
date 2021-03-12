using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
#nullable disable

namespace BillingEnd.Models
{
    public partial class Pedido
    {
        public int Id { get; set; }
        public decimal? Medida { get; set; }
        public string Marca { get; set; }
        public int? Cantidad { get; set; }
        public decimal? Unitario { get; set; }
        [Display(Name = "Valor Bulto que VENDI")]
        public decimal? Bulto { get; set; }
        public string Etiqueta { get; set; }
        [Display(Name = "Valor Bulto que COMPRE")]
        public int? CantidadDeBultos { get; set; }
        public decimal? Total { get; set; }
        public int? Factura { get; set; }
        public int? Producto { get; set; }

        public virtual Factura FacturaNavigation { get; set; }
    }
}
