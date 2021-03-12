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
using iTextSharp.text.html.simpleparser;
using System.Text.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web;


namespace BillingDatabase.Controllers
{
    public class PedidoController : Controller
    {
        private readonly db _context;

        List<Pedido> li = new List<Pedido>();


        public PedidoController(db context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart");
            if(cart == null){return View("VistaVacia");}
            ViewBag.cart = cart;
            ViewBag.Bulto = cart.Sum(x => x.PrdoBulto = x.ProdUnitario * x.ProdCantidad);
            ViewBag.total = cart.Sum(item => item.Total = item.CantidadDeBultosPedido * ViewBag.Bulto);
            var porcentajeVendor = ViewBag.total * 0.02;
            ViewBag.porcentajeVendor = porcentajeVendor;
            var totalganancia = ViewBag.total+=ViewBag.total;
            ViewBag.totalganancia = totalganancia;
            return View(cart);
        }
        private int isExist(string id)
        {
            List<Producto> cart = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart");
            var i = cart.FindIndex(item => item.ProdId == Convert.ToInt32(id));
            if (i != -1)
            {
                return i;
            }

            return -1;
        }

        [HttpPost, ActionName("ObtenerNombre")]
        public IActionResult ObtenerNombre(string name, string direccion)
        {
            ViewBag.Name = name;
            ViewBag.Direccion = direccion;
            return RedirectToAction("Exportar", "Pedido");
        }


        public IActionResult Adtocart(string id, int cantidadaapedir)
        {

            Producto pedido = _context.Productos
             .Where(a => a.ProdId == Convert.ToInt32(id))
             .FirstOrDefault();
            if (SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart") == null)
            {
                List<Producto> cart = new List<Producto>();
                cart.Add(new Producto
                {
                    ProdId = Convert.ToInt32(id),
                    ProdCantidad = pedido.ProdCantidad,
                    ProdMarca = pedido.ProdMarca,
                    ProdEtiqueta = pedido.ProdEtiqueta,
                    CantidadDeBultosPedido = cantidadaapedir,
                    ProdMedida = pedido.ProdMedida,
                    ProdUnitario = pedido.ProdUnitario,
                    PrdoBulto = pedido.PrdoBulto,
                    Total = Convert.ToInt32(cantidadaapedir) * pedido.PrdoBulto
                });
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            }
            else
            {
                List<Producto> cart = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart");
                var index = isExist(id.ToString());
                if (index != -1)
                {
                    cart[index].CantidadDeBultosPedido++;

                }
                else
                {

                    cart.Add(new Producto
                    {
                        ProdId = Convert.ToInt32(id),
                        ProdCantidad = pedido.ProdCantidad,
                        ProdMarca = pedido.ProdMarca,
                        ProdEtiqueta = pedido.ProdEtiqueta,
                        ProdMedida = pedido.ProdMedida,
                        ProdUnitario = pedido.ProdUnitario,
                        PrdoBulto = pedido.PrdoBulto,
                        CantidadDeBultosPedido = cantidadaapedir
                    });

                }
                SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);

            }
            return RedirectToAction("index");
        }

        public IActionResult Remove(string id)
        {

            var cart = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart").ToList();
            Producto c = cart.Where(x => x.ProdId == Convert.ToInt32(id)).FirstOrDefault();
            var total = ViewBag.total;
            var index = isExist(id);
            decimal h = 0;

            if (index != -1)
            {
                cart[index].CantidadDeBultosPedido--;
                if (cart[index].CantidadDeBultosPedido == 0) { cart.Remove(c); }
            }
            else { cart.Remove(c); }



            ViewBag.total = h;
            SessionHelper.SetObjectAsJson(HttpContext.Session, "cart", cart);
            return RedirectToAction("Index");

        }

        public IActionResult Exportar()
        {
            var cart = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart");
            ViewBag.cart = cart;
            ViewBag.Bulto = cart.Sum(x => x.PrdoBulto = x.ProdUnitario * x.ProdCantidad);
            ViewBag.total = cart.Sum(item => item.Total = item.CantidadDeBultosPedido * ViewBag.Bulto);
            var porcentajeVendor = ViewBag.total * 0.02;
            ViewBag.porcentajeVendor = porcentajeVendor;
            return View(cart);
        }

      
        [HttpPost, ActionName("Export")]

        public FileResult Export(string name, string direccion)
        {
            var cart = SessionHelper.GetObjectFromJson<List<Producto>>(HttpContext.Session, "cart");
            ViewBag.Bulto = cart.Sum(x => x.PrdoBulto = x.ProdUnitario * x.ProdCantidad);
            var total = cart.Sum(item => item.Total = item.CantidadDeBultosPedido * ViewBag.Bulto);


            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                using (Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 10f))
                {
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();

                    iTextSharp.text.Font _standardFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.NORMAL, BaseColor.BLACK);

                    pdfDoc.Add(new Paragraph("PEDIDO REALIZADO"));
                    pdfDoc.Add(Chunk.NEWLINE);

                    PdfPTable tblPrueba = new PdfPTable(8);
                    tblPrueba.WidthPercentage = 100;

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

                    PdfPCell clCantidadBultosPedidos = new PdfPCell(new Phrase("Cantidad de bultos", _standardFont));
                    clCantidadBultosPedidos.BorderWidth = 0;
                    clCantidadBultosPedidos.BorderWidthBottom = 0.75f;
                    clCantidadBultosPedidos.BackgroundColor = BaseColor.CYAN;
                    clCantidadBultosPedidos.Border = 0;
                    clCantidadBultosPedidos.HorizontalAlignment = 1;

                    PdfPCell clTotal = new PdfPCell(new Phrase("Total", _standardFont));
                    clTotal.BorderWidth = 0;
                    clTotal.BorderWidthBottom = 0.75f;
                    clTotal.BackgroundColor = BaseColor.CYAN;
                    clTotal.Border = 0;
                    clTotal.HorizontalAlignment = 1;

                    tblPrueba.AddCell(clId);
                    tblPrueba.AddCell(clMarca);
                    tblPrueba.AddCell(clCantidad);
                    tblPrueba.AddCell(clMedida);
                    tblPrueba.AddCell(clUnitario);
                    tblPrueba.AddCell(clBulto);
                    tblPrueba.AddCell(clEtiqueta);
                    tblPrueba.AddCell(clCantidadBultosPedidos);

                    var listaprod = _context.Productos.ToList();

                    foreach (var item in cart)
                    {
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

                        clBulto = new PdfPCell(new Phrase(ViewBag.Bulto.ToString(), _standardFont));
                        clBulto.HorizontalAlignment = 1;

                        clEtiqueta = new PdfPCell(new Phrase(item.ProdEtiqueta.ToString(), _standardFont));
                        clEtiqueta.HorizontalAlignment = 1;

                        clCantidadBultosPedidos = new PdfPCell(new Phrase(item.CantidadDeBultosPedido.ToString(), _standardFont));
                        clCantidadBultosPedidos.HorizontalAlignment = 1;




                        tblPrueba.AddCell(clId);
                        tblPrueba.AddCell(clMarca);
                        tblPrueba.AddCell(clCantidad);
                        tblPrueba.AddCell(clMedida);
                        tblPrueba.AddCell(clUnitario);
                        tblPrueba.AddCell(clBulto);
                        tblPrueba.AddCell(clEtiqueta);
                        tblPrueba.AddCell(clCantidadBultosPedidos);

                    }

                    // if (direccion.ToString() == "null" && name.ToString() == "null")
                    // {
                    //     pdfDoc.Add(tblPrueba);
                    //     pdfDoc.Add(new Paragraph("Total : " + total.ToString()));
                    //     pdfDoc.Add(new Paragraph("Nombre Vendor : no completado"));
                    //     pdfDoc.Add(new Paragraph("Direccion : no completada "));
                    //     pdfDoc.Close();
                    //     writer.Close();

                    // }

                    pdfDoc.Add(tblPrueba);
                    pdfDoc.Add(new Paragraph("Total : " + total.ToString()));
                    pdfDoc.Add(new Paragraph("Nombre Vendor : " + name.ToString()));
                    pdfDoc.Add(new Paragraph("Direccion : " + direccion.ToString()));


                    pdfDoc.Close();
                    writer.Close();



                    DateTime date = DateTime.Now;

                    return File(stream.ToArray(), "application/pdf", $"Pedido/{direccion.ToString()}/{date}.pdf");
                }
            }
        }






    }
}