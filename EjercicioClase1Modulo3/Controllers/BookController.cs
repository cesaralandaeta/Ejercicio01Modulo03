using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text.Json;

namespace EjercicioClase1Modulo3.Controllers
{
    
    [ApiController]
    [Route("v1/libros")]
    public class BookController : ControllerBase
    {
        //Books contiene una lista de libros. Esta información viene del archivo libros.json ubicado dentro de la carpeta Data.
        public List<Book> Books { get; set; }

        //filePath contiene la ubicación del archivo libros.json. No mover el archivo libros.json de esa carpeta.
        string filePath = Path.Combine( AppDomain.CurrentDomain.BaseDirectory, @"Data\libros.json" );

        public BookController()
        {
            //Instanciación e inicialización de la lista de libros deserializando el archivo libros.json
            Books = JsonSerializer.Deserialize<List<Book>>( System.IO.File.ReadAllText( filePath ) );
        }

        #region Ejercicio 1
        /*
        Completar y modificar el método siguiente para crear un endpoint que liste todos los libros y tenga la siguiente estructura:
        [GET] v1/libros
        */

        [HttpGet]
        //[Route( "v1/libros" )]
        public ActionResult<List<Book>> GetBooks()
        {
            return Ok(Books.FirstOrDefault());
        }

        #endregion

        #region Ejercicio 2
        /*
         Crear un endpoint para Obtener un libro por su número de id usando route parameters que tenga la siguiente estructura:
        [GET] v1/libros/{id}
        Ejemplo: v1/libros/8 (devuelve toda la información del libro cuyo id es 8. Es decir: El diario de Ana Frank)
        */

        [HttpGet]
        [Route("{IdLibro}")]
        public ActionResult<List<Book>> GetBookId([FromRoute] int idLibro)
        {
            var book = Books.FirstOrDefault(b => b.id == idLibro);
            return Ok(book);

        }
        #endregion

        #region Ejercicio 3
        /*
         Crear un endpoint para listar todos libros de un género en particular usando route parameters que tenga la siguiente estructura:
        [GET] v1/libros/genero/{genero} 
        Ejemplo: v1/libros/genero/fantasía (devuelve una lista de todos los libros del género fantasía)
         */
        [HttpGet]
        [Route("genero/{genero}")]
        public ActionResult<List<Book>> GetBookPorGenero([FromRoute] string genero)
        {
            var bookPorGenero = Books.FirstOrDefault(g => g.genero == genero);

            return Ok(bookPorGenero);
        }

        #endregion

        #region Ejercicio 4
        /*
        Crear un endpoint para Listar todos los libros de un autor usando query parameters que tenga la siguiente estructura:
        [GET] v1/libros?autor={autor}
        Ejemplo: v1/libros?autor=Paulo Coelho (devuelve una lista de todos los libros del autor Paulo Coelho)
         */
        [HttpGet]
        [Route("autor")]
        public ActionResult<List<Book>> GetBookPorAutor([FromQuery] string autor)
        {
            var bookBuscarPorAutor = Books.FirstOrDefault(a => a.autor == autor);

            return Ok(bookBuscarPorAutor);  
        }

        #endregion

        #region Ejercicio 5
        /*
        Crear un endpoint para Listar unicamente todos los géneros de libros disponibles que tenga la siguiene estructura:
        [GET] v1/libros/generos
        Idealmente, el listado de géneros que retorne el endpoint, no debe contener repetidos.
         */
        [HttpGet]
        [Route("generos")]
        public ActionResult<List<Book>> GetListadoGenero()
        {
            var generos = Books.Select(l => l.genero).Distinct().ToList();
            return Ok(generos);   
        }
        #endregion

        #region Ejercicio 6
        /*
        Crear un endpoint para listar todos los libros implementando paginación usando route parameters con la siguiente estructura:
        [GET] v1/libros?pagina={numero-pagina}&cantidad={cantidad-por-pagina}
        Ejemplos: 
        v1/libros?pagina=1&cantidad=10 (devuelve una lista de los primeros diez libros)
        v1/libros?pagina=2&cantidad=10 (devuelve una lista de diez libros, salteando los primeros 10)
        v1/libros?pagina=3&cantidad=10 (devuelve una lista de diez libros, salteando los primeros 20)
         */
        [HttpGet]
        [Route("pagina")]
        public ActionResult<List<Book>> GetLibros([FromQuery] int pagina = 1, [FromQuery] int cantidad = 10)
        {
            //if (pagina < 1 || cantidad < 1)
            //{
            //    return BadRequest(new { mensaje = "Los parámetros 'pagina' y 'cantidad' deben ser mayores que 0" });
            //}

            var librosPaginados = Books.Skip((pagina - 1) * cantidad).Take(cantidad).ToList();

            return Ok(librosPaginados);
        }
        #endregion
    }
}
