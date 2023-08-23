using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{

    public class ProductsController : CustomBaseController
    {
        
        private readonly IProductService _service;
        public ProductsController(IProductService productService)
        {
            _service = productService;
        
        }

        /// <summary>
        /// GET api/products/getProductsWithCategory
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> getProductsWithCategory()
        {
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        [HttpGet]
        public async Task<IActionResult> All()
        {
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200,productsDtos));
            return CreateActionResult(await _service.GetAllAsync());
        }

        /// <summary>
        /// Service Filter because there is an dependency injection 
        /// typeof because it is a generic class filter
        /// </summary>

        [ServiceFilter(typeof(NotFoundFilter<Product,BaseDto>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200,productsDtos));
            return CreateActionResult(await _service.GetByIdAsync(id));
        }

        [HttpPost()]
        public async Task<IActionResult> Save(ProductCreateDto productDto)
        {

            return CreateActionResult(await _service.AddAsync(productDto));
        }

        [HttpPut()]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            return CreateActionResult(await _service.UpdateAsync(productUpdateDto));
        }
        [ServiceFilter(typeof(NotFoundFilter<Product, BaseDto>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {          
            return CreateActionResult(await _service.RemoveAsync(id));
        }
    }
}
