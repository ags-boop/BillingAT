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
    public class GananciaController : Controller
    {
        private readonly db _context;
        // private readonly Helper Helper;

        List<Pedido> li = new List<Pedido>();


        public GananciaController(db context)
        {
            _context = context;
            // _helper=Helper;
        }
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



        public IActionResult Index(string campobusquedad)
        {
            bool IsValid = (bool)TempData["IsValid"];
            TempData.Keep("IsValid");
            if(IsValid)
            {
                var ProductosDB = _context.Pedidos.Select(p => new Pedido()
                {
                    Id = p.Id,
                    Medida = p.Medida,
                    Marca = p.Marca,
                    Etiqueta = p.Etiqueta,
                    CantidadDeBultos = p.CantidadDeBultos,
                    Bulto = p.Bulto,
                    Total = p.Bulto - p.CantidadDeBultos
                }).ToList();
                var busqueda = from s in ProductosDB select s;

                if (!String.IsNullOrEmpty(campobusquedad))
                {
                    busqueda = busqueda.Where(s => s.Etiqueta.Contains(campobusquedad) || s.Marca.Contains(campobusquedad) || s.Medida.ToString().Contains(campobusquedad));
                    return View(busqueda.ToList());
                }
                return View(ProductosDB);
            }
            return View("NoAuth");
            


        }

        public IActionResult Crear()
        {
            ViewBag.MedidasElegir = MedidasElegir;
            ViewBag.EtiquetaAElegir = EtiquetaAElegir;
            return View();
        }

        [HttpPost]

        public IActionResult Crear(Pedido ProductoGanancia)
        {
            if (ModelState.IsValid)
            {
                ProductoGanancia.Id = _context.Pedidos.Max(X => X.Id + 1);
                _context.Pedidos.Add(ProductoGanancia);
                _context.SaveChanges();
                return RedirectToAction("Index", "Ganancia");
            }
            return View(ProductoGanancia);
        }

        public IActionResult Editar(int id)
        {
            ViewBag.MedidasElegir = MedidasElegir;
            ViewBag.EtiquetaAElegir = EtiquetaAElegir;
            var ProductoDB = _context.Pedidos.Where(x => x.Id == id).FirstOrDefault();
            return View(ProductoDB);
        }

        [HttpPost]
        public IActionResult Editar(Pedido ProductoGanancia)
        {

            // if(Username == "admin@domain.com")
            // {
            if (ModelState.IsValid)
            {
                // string carpetaFotos = Path.Combine(_hostingEnvironment.WebRootPath, "ImagenesPerros");
                // string nombreArchivo = Perro.Foto.FileName;
                // string rutaCompleta = Path.Combine(carpetaFotos, nombreArchivo);
                // //Img al servidor
                // Perro.Foto.CopyTo(new FileStream(rutaCompleta, FileMode.Create));
                //Guardamos la ruta de la img
                // Perro.FotoRuta = nombreArchivo;
                _context.Pedidos.Update(ProductoGanancia);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(ProductoGanancia);

        }

        public IActionResult Detalle(int id)
        {
            var ProductoGanancia = _context.Pedidos
             .Where(a => a.Id == id)
             .FirstOrDefault();
            return View(ProductoGanancia);
        }



        [HttpPost, ActionName("Borrar")]
        public IActionResult Borrar(int id)
        {
            Pedido ProductoGananciaBorrar = new Pedido { Id = id };
            _context.Remove(ProductoGananciaBorrar).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }






    }
}