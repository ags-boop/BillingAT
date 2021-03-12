using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BillingEnd.Models
{
    public partial class Producto
    {
        [Display(Name = "ID PRODUCTO")]
        public int ProdId { get; set; }
        [Display(Name = "MARCA")]

        public string ProdMarca { get; set; }
        [Display(Name = "ETIQUETA")]

        public string ProdEtiqueta { get; set; }
        [Display(Name = "MEDIDA")]

        public double? ProdMedida { get; set; }
        [Display(Name = "CANTIDAD POR BULTO")]

        public int? ProdCantidad { get; set; }
        [Display(Name = "PRECIO UNITARIO")]

        public double? ProdUnitario { get; set; }
        [Display(Name = "VALOR DEL BULTO")]

        public double? PrdoBulto { get; set; }
        [Display(Name = "TOTAL ")]

        public double? Total { get; set; }
        [Display(Name = "CANTIDAD DE BULTOS PEDIDOS")]

        public int? CantidadDeBultosPedido { get; set; }
        public double? CompraBulto { get; set; }
    }
}
