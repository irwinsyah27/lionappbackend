using AppBE.Models;
using AppBE.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AppBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("_myAllowSpecificOrigins")]

    public class ProductController : ControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IWebHostEnvironment _environment;

        public ProductController(IRepository<Product> productRepository)
        {
            _productRepository = productRepository;
        }


        [HttpGet]
        public IActionResult Get()
        {
            IEnumerable<Product> products = _productRepository.GetAll();
            return Ok(products);
        }


        [HttpGet("{productId}")]
        public IActionResult Get(int productId)
        {
            var product = _productRepository.GetById(productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post(Product product)
        {
            _productRepository.Add(product);
            return Ok();
        }

        [HttpPut("{productId}")]
        public IActionResult Put(Product updatedProduct)
        {
            bool updated = _productRepository.Update(updatedProduct);
            if (updated)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            bool deleted = _productRepository.Delete(productId);
            if (deleted)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }



        [NonAction]
        private string GetFilePath(string ProductID)
        {
            return this._environment.WebRootPath + "\\Uploads\\Product\\" + ProductID;
        }

        [NonAction]
        private string GetImagebyProduct(string productid)
        {
            string ImageUrl = string.Empty;
            string HostUrl = "https://localhost:5128/";
            string Filepath = GetFilePath(productid);
            string Imagepath = Filepath + "\\image.png";
            if (!System.IO.File.Exists(Imagepath))
            {
                ImageUrl = HostUrl + "/Uploads/common/noimage.png";
            }
            else
            {
                ImageUrl = HostUrl + "/Uploads/Product/" + productid + "/image.png";
            }
            return ImageUrl;

        }
    }
}