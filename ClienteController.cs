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
    public class ClienteController : Controller
    {
        private readonly db _context;
        // private readonly Helper Helper;

        List<Pedido> li = new List<Pedido>();


        public ClienteController(db context)
        {
            _context = context;
            // _helper=Helper;
        }



        public IActionResult Index(string campobusquedad)
        {

            var ClienteDB = _context.Clientes.Select(p => new Cliente()
            {
                CliId = p.CliId,
                CliNum = p.CliNum,
                CliDireccion = p.CliDireccion,
                CliDni = p.CliDni,
                CliCuil = p.CliCuil,

            }).ToList();
            var busqueda = from s in _context.Clientes select s;

            if (!String.IsNullOrEmpty(campobusquedad))
            {
                busqueda = busqueda.Where(s => s.CliDireccion.Contains(campobusquedad));
                return View(busqueda.ToList());
            }
            return View(ClienteDB);


        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]

        public IActionResult Crear(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente.CliId = _context.Clientes.Max(X => X.CliId + 1);
                _context.Clientes.Add(cliente);
                _context.SaveChanges();
                return RedirectToAction("Index", "Cliente");
            }
            return View(cliente);
        }

        public IActionResult Editar(int id)
        {
          
            var ProductoDB = _context.Clientes.Where(x => x.CliId == id).FirstOrDefault();
            return View(ProductoDB);
        }

        [HttpPost]
        public IActionResult Editar(Cliente ClientePost)
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
                _context.Clientes.Update(ClientePost);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
                return View(ClientePost);

        }

        public IActionResult Detalle(int id)
        {
            var ClientePost = _context.Clientes
             .Where(a => a.CliId == id)
             .FirstOrDefault();
            return View(ClientePost);
        }



        [HttpPost, ActionName("Borrar")]
        public IActionResult Borrar(int id)
        {
            Cliente ClienteBorrar = new Cliente { CliId = id };
            _context.Remove(ClienteBorrar).State = EntityState.Deleted;
            _context.SaveChanges();
            return RedirectToAction("Index");
        }






    }
}