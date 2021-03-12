using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace BillingEnd.Models
{
    public partial class db : DbContext
    {
        public db()
        {
        }

        public db(DbContextOptions<db> options)
            : base(options)
        {
        }

        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Factura> Facturas { get; set; }
        public virtual DbSet<Pedido> Pedidos { get; set; }
        public virtual DbSet<Producto> Productos { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                // optionsBuilder.UseSqlServer("Server=DESKTOP-8Q85QCQ;Database=BillingDatabase;trusted_connection=true; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasKey(e => e.CliId)
                    .HasName("PK__Cliente__FFEFE14F9FB7B947");

                entity.ToTable("Cliente");

                entity.Property(e => e.CliId)
                    .ValueGeneratedNever()
                    .HasColumnName("cli_id");

                entity.Property(e => e.CliCuil)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cli_cuil");

                entity.Property(e => e.CliDireccion)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cli_direccion");

                entity.Property(e => e.CliDni)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cli_dni");

                entity.Property(e => e.CliNum)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cli_num");
                entity.Property<string>("CliNombre")
                        .HasColumnType("nvarchar(max)");
            });

            modelBuilder.Entity<Factura>(entity =>
            {
                entity.HasKey(e => e.FacId)
                    .HasName("PK__Factura__978BA2C3393726D7");

                entity.ToTable("Factura");

                entity.Property(e => e.FacId)
                    .ValueGeneratedNever()
                    .HasColumnName("fac_id");

                entity.Property(e => e.FacDate)
                    .HasColumnType("datetime")
                    .HasColumnName("fac_date");

                entity.Property(e => e.FacFkUser).HasColumnName("fac_fk_user");

                entity.Property(e => e.FacTotal).HasColumnName("fac_total");

                entity.HasOne(d => d.FacFkUserNavigation)
                    .WithMany(p => p.Facturas)
                    .HasForeignKey(d => d.FacFkUser)
                    .HasConstraintName("FK_Factura_Cliente");
            });

            modelBuilder.Entity<Pedido>(entity =>
            {
                entity.ToTable("Pedido");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.Bulto).HasColumnType("money");

                entity.Property(e => e.CantidadDeBultos).HasColumnName("Cantidad_De_Bultos");

                entity.Property(e => e.Etiqueta)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Factura).HasColumnName("factura");

                entity.Property(e => e.Marca)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Medida).HasColumnType("money");

                entity.Property(e => e.Producto).HasColumnName("producto");

                entity.Property(e => e.Total).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Unitario).HasColumnType("money");

                entity.HasOne(d => d.FacturaNavigation)
                    .WithMany(p => p.Pedidos)
                    .HasForeignKey(d => d.Factura)
                    .HasConstraintName("FK_Pedido_Factura");
            });

            modelBuilder.Entity<Producto>(entity =>
            {
                entity.HasKey(e => e.ProdId);

                entity.ToTable("Producto");

                entity.Property(e => e.ProdId)
                    .ValueGeneratedNever()
                    .HasColumnName("prod_id");

                entity.Property(e => e.PrdoBulto).HasColumnName("prdo_bulto");

                entity.Property(e => e.ProdCantidad).HasColumnName("prod_cantidad");

                entity.Property(e => e.ProdEtiqueta)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("prod_etiqueta");

                entity.Property(e => e.ProdMarca)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("prod_marca");

                entity.Property(e => e.ProdMedida).HasColumnName("prod_medida");

                entity.Property(e => e.ProdUnitario).HasColumnName("prod_unitario");
                
                entity.Property<double?>("CompraBulto").HasColumnType("float");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
