using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using BillingEnd.Models;
using Microsoft.EntityFrameworkCore;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;
using iTextSharp.text.html.simpleparser;
using X.PagedList;

namespace BillingMockUpSystem.Controllers
{
    public class ProductoController : Controller
    {
        private readonly db _context;

        public List<Medidas> MedidasElegir = new List<Medidas>(){
            new Medidas(){ID=1,Medida=330},
            new Medidas(){ID=2,Medida=473},
            new Medidas(){ID=3,Medida=500},
            new Medidas(){ID=4,Medida=600},
            new Medidas(){ID=5,Medida=710},
            new Medidas(){ID=6,Medida=730},
            new Medidas(){ID=7,Medida=750},
            new Medidas(){ID=8,Medida=1.0m},
            new Medidas(){ID=9,Medida=1.25m},
            new Medidas(){ID=10,Medida=1.5m},
            new Medidas(){ID=11,Medida=1.75m},
            new Medidas(){ID=12,Medida=2.0m},
            new Medidas(){ID=13,Medida=2.25m},
            new Medidas(){ID=14,Medida=2.5m},
            new Medidas(){ID=15,Medida=2.75m},
            new Medidas(){ID=16,Medida=3},
            new Medidas(){ID=17,Medida=6},
            new Medidas(){ID=18,Medida=269},
            new Medidas(){ID=19,Medida=355},
            new Medidas(){ID=20,Medida=354},




        };

        public List<Etiqueta> EtiquetaAElegir = new List<Etiqueta>(){
            new Etiqueta(){ID=1,Etiquetas="Gaseosa"},
            new Etiqueta(){ID=2,Etiquetas="Cerveza"},
            new Etiqueta(){ID=3,Etiquetas="Isotonico"},
            new Etiqueta(){ID=4,Etiquetas="Agua"},
            new Etiqueta(){ID=5,Etiquetas="Vino"},
            new Etiqueta(){ID=6,Etiquetas="Energizante"},
            new Etiqueta(){ID=7,Etiquetas="Agua Saborizada"},
            new Etiqueta(){ID=8,Etiquetas="Aperitivos"},


        };




        public ProductoController(db context)
        {
            _context = context;
        }

        // [HttpPost,ActionName("Index")]

        public IActionResult Index(string campobusquedad,string CantidadDeBultosPedido)
        {
            var ProductosDB = _context.Productos.Select(p => new Producto()
            {
                ProdId = p.ProdId,
                ProdMedida = p.ProdMedida,
                ProdMarca = p.ProdMarca,
                ProdCantidad = p.ProdCantidad,
                ProdUnitario = p.ProdUnitario,
                PrdoBulto = p.ProdCantidad*p.ProdUnitario,
                ProdEtiqueta = p.ProdEtiqueta
            }).ToList();

            ViewBag.CantidadDeBultosPedido = CantidadDeBultosPedido;
            var busqueda = from s in ProductosDB select s;

            if (!String.IsNullOrEmpty(campobusquedad))
            {
                busqueda = busqueda.Where(s => s.ProdEtiqueta.Contains(campobusquedad) || s.ProdMarca.Contains(campobusquedad) || s.ProdMedida.ToString().Contains(campobusquedad));
                return View(busqueda.ToList());
            }

            return View(ProductosDB);
        }




        public IActionResult Exportar()
        {
             var ProductosDB = _context.Productos.Select(p => new Producto()
            {
                ProdId = p.ProdId,
                ProdMedida = p.ProdMedida,
                ProdMarca = p.ProdMarca,
                ProdCantidad = p.ProdCantidad,
                ProdUnitario = p.ProdUnitario,
                PrdoBulto = p.ProdCantidad*p.ProdUnitario,
                ProdEtiqueta = p.ProdEtiqueta
            }).ToList();
            SessionHelper.SetObjectAsJson(HttpContext.Session,"ProductList",ProductosDB);
            return View(ProductosDB);
        }
        public FileResult Export()
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                var listaprod = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session,"ProductList");

                //Initialize the PDF document object.
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    //Add the Image file to the PDF document object.
                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    // Escribimos el encabezamiento en el documento
                    pdfDoc.Add(new Paragraph("Lista de Productos"));
                    pdfDoc.Add(Chunk.NEWLINE);

                    // Creamos una tabla que contendrá el nombre, apellido y país
                    // de nuestros visitante.
                    PdfPTable tblPrueba = new PdfPTable(7);
                    tblPrueba.WidthPercentage = 100;

                    // Configuramos el título de las columnas de la tabla
                    PdfPCell clId = new PdfPCell(new Phrase("Id", _standardFont));
                    clId.BorderWidth = 0;
                    clId.BorderWidthBottom = 0.75f;
                    clId.BackgroundColor = BaseColor.CYAN;
                    clId.Border = 1;
                    clId.HorizontalAlignment = 1;


                    PdfPCell clMarca = new PdfPCell(new Phrase("Marca", _standardFont));
                    clMarca.BorderWidth = 0;
                    clMarca.BorderWidthBottom = 0.75f;
                    clMarca.BackgroundColor = BaseColor.CYAN;
                    clMarca.Border = 1;
                    clMarca.HorizontalAlignment = 1;

                    PdfPCell clCantidad = new PdfPCell(new Phrase("Cantidad", _standardFont));
                    clCantidad.BorderWidth = 0;
                    clCantidad.BorderWidthBottom = 0.75f;
                    clCantidad.BackgroundColor = BaseColor.CYAN;
                    clCantidad.Border = 1;
                    clCantidad.HorizontalAlignment = 1;



                    PdfPCell clMedida = new PdfPCell(new Phrase("Medida", _standardFont));
                    clMedida.BorderWidth = 0;
                    clMedida.BorderWidthBottom = 0.75f;
                    clMedida.BackgroundColor = BaseColor.CYAN;
                    clMedida.Border = 1;
                    clMedida.HorizontalAlignment = 1;


                    PdfPCell clUnitario = new PdfPCell(new Phrase("Unitario", _standardFont));
                    clUnitario.BorderWidth = 0;
                    clUnitario.BorderWidthBottom = 0.75f;
                    clUnitario.BackgroundColor = BaseColor.CYAN;
                    clUnitario.Border = 1;
                    clUnitario.HorizontalAlignment = 1;


                    PdfPCell clBulto = new PdfPCell(new Phrase("Bulto", _standardFont));
                    clBulto.BorderWidth = 0;
                    clBulto.BorderWidthBottom = 0.75f;
                    clBulto.BackgroundColor = BaseColor.CYAN;
                    clBulto.Border = 0;
                    clBulto.HorizontalAlignment = 1;

                    PdfPCell clEtiqueta = new PdfPCell(new Phrase("Etiqueta", _standardFont));
                    clEtiqueta.BorderWidth = 0;
                    clEtiqueta.BorderWidthBottom = 0.75f;
                    clEtiqueta.BackgroundColor = BaseColor.CYAN;
                    clEtiqueta.Border = 0;
                    clEtiqueta.HorizontalAlignment = 1;

                    // Añadimos las celdas a la tabla
                    tblPrueba.AddCell(clId);
                    tblPrueba.AddCell(clMarca);
                    tblPrueba.AddCell(clCantidad);
                    tblPrueba.AddCell(clMedida);
                    tblPrueba.AddCell(clUnitario);
                    tblPrueba.AddCell(clBulto);
                    tblPrueba.AddCell(clEtiqueta);

                    // var listaprod = ViewBag.ProductosDB;

                    foreach (var item in listaprod)
                    {
                        // Llenamos la tabla con información
                        clId = new PdfPCell(new Phrase(item.ProdId.ToString(), _standardFont));
                        clId.HorizontalAlignment = 1;

                        clMarca = new PdfPCell(new Phrase(item.ProdMarca, _standardFont));
                        clMarca.HorizontalAlignment = 1;

                        clCantidad = new PdfPCell(new Phrase(item.ProdCantidad.ToString(), _standardFont));
                        clCantidad.HorizontalAlignment = 1;

                        clMedida = new PdfPCell(new Phrase(item.ProdMedida.ToString(), _standardFont));
                        clMedida.HorizontalAlignment = 1;

                        clUnitario = new PdfPCell(new Phrase(item.ProdUnitario.ToString(), _standardFont));
                        clUnitario.HorizontalAlignment = 1;

                        clBulto = new PdfPCell(new Phrase(item.PrdoBulto.ToString(), _standardFont));
                        clBulto.HorizontalAlignment = 1;

                        clEtiqueta = new PdfPCell(new Phrase(item.ProdEtiqueta.ToString(), _standardFont));
                        clEtiqueta.HorizontalAlignment = 1;


                        // Añadimos las celdas a la tabla
                        tblPrueba.AddCell(clId);
                        tblPrueba.AddCell(clMarca);
                        tblPrueba.AddCell(clCantidad);
                        tblPrueba.AddCell(clMedida);
                        tblPrueba.AddCell(clUnitario);
                        tblPrueba.AddCell(clBulto);
                        tblPrueba.AddCell(clEtiqueta);



                    }
                    pdfDoc.Add(tblPrueba);

                    pdfDoc.Close();
                    writer.Close();





                    //Download the PDF file.
                    return File(stream.ToArray(), "application/pdf", "PdfExportList.pdf");
                }
            }

        }


        public IActionResult Crear()
        {
            ViewBag.MedidasElegir = MedidasElegir;

            return View();
        }

        [HttpPost]
        public IActionResult Crear(Producto ProductoPost)
        {
            bool IsValid = (bool)TempData["IsValid"];
            TempData.Keep("IsValid");

            if(IsValid)
            {
                if (ModelState.IsValid)
                {
                
                    ViewBag.MedidasElegir = MedidasElegir;
                    ProductoPost.ProdId = _context.Productos.Max(X => X.ProdId + 1);
                    ProductoPost.PrdoBulto = ProductoPost.ProdUnitario*ProductoPost.ProdCantidad;
                    _context.Productos.Add(ProductoPost);
                    _context.SaveChanges();
                    return RedirectToAction("Index");


                }
            }
            else{return View("NoAuth");}
            return View(ProductoPost);




        }

        public IActionResult Editar(int id)
        {
            ViewBag.MedidasElegir = MedidasElegir;
            ViewBag.EtiquetaAElegir = EtiquetaAElegir;
            var ProductoDB = _context.Productos.Where(x => x.ProdId == id).FirstOrDefault();
            return View(ProductoDB);
        }

        [HttpPost]
        public IActionResult Editar(Producto ProductoPost)
        {
            ViewBag.MedidasElegir = MedidasElegir;
            ViewBag.EtiquetaAElegir = EtiquetaAElegir;
            bool IsValid = (bool)TempData["IsValid"];
            TempData.Keep("IsValid");

            if(IsValid)
            {
                if (ModelState.IsValid)
                {
                    // string carpetaFotos = Path.Combine(_hostingEnvironment.WebRootPath, "ImagenesPerros");
                    // string nombreArchivo = Perro.Foto.FileName;
                    // string rutaCompleta = Path.Combine(carpetaFotos, nombreArchivo);
                    // //Img al servidor
                    // Perro.Foto.CopyTo(new FileStream(rutaCompleta, FileMode.Create));
                    //Guardamos la ruta de la img
                    // Perro.FotoRuta = nombreArchivo;
                    ProductoPost.PrdoBulto = ProductoPost.ProdUnitario*ProductoPost.ProdCantidad;
                    _context.Productos.Update(ProductoPost);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View("NoAuth");
            }
            else
                return View(ProductoPost);

        }

        public IActionResult Detalle(int id)
        {
            var ProductoPost = _context.Productos
             .Where(a => a.ProdId == id)
             .FirstOrDefault();
            
            return View(ProductoPost);
        }


        
        [HttpPost, ActionName("Borrar")]
        public IActionResult Borrar(Producto id)
        {
            bool IsValid = (bool)TempData["IsValid"];
            TempData.Keep("IsValid");
            if(IsValid)
            {
                Producto Delete = new Producto { ProdId = id.ProdId };
                _context.Remove(Delete).State = EntityState.Deleted;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("NoAuth");
        }

        public IActionResult Order(string campobusquedad)
        {
            var busqueda = from s in _context.Productos select s;

            if (!String.IsNullOrEmpty(campobusquedad))
            {
                busqueda = busqueda.Where(s => s.ProdEtiqueta.Contains(campobusquedad));

                return View(busqueda.ToList());
            }
            var PerroDB = _context.Productos.OrderByDescending(c => c.ProdId).ToList();
            return View(PerroDB);
        }

         private IPagedList Paginar(List<Producto> Prod, int? pagina)
        {
            int tamanioPag = 6;
            int numeroPagina = (pagina ?? 1);
            return Prod.ToPagedList(numeroPagina, tamanioPag);
        }

        public IActionResult Listado(int? pagina)
        {
            return View(Paginar(_context.Productos.ToList(), pagina));
        }




    }

}