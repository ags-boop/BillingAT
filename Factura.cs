using System;
using System.Collections.Generic;

#nullable disable

namespace BillingEnd.Models
{
    public partial class Factura
    {
        public Factura()
        {
            Pedidos = new HashSet<Pedido>();
        }

        public int FacId { get; set; }
        public int? FacFkUser { get; set; }
        public DateTime? FacDate { get; set; }
        public double? FacTotal { get; set; }

        public virtual Cliente FacFkUserNavigation { get; set; }
        public virtual ICollection<Pedido> Pedidos { get; set; }
    }
}
