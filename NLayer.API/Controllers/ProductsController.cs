﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NLayer.API.Filters;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{

    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        
        private readonly IProductService _service;
        public ProductsController(IMapper mapper, IProductService productService)
        {
            _mapper = mapper;
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
            var products = await _service.GetAllAsync();
            var productsDtos = _mapper.Map<List<ProductDto>>(products.ToList());

            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200,productsDtos));
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Success(200, productsDtos));
        }

        /// <summary>
        /// Service Filter because there is an dependency injection 
        /// typeof because it is a generic class filter
        /// </summary>

        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productsDto = _mapper.Map<ProductDto>(product);

            //return Ok(CustomResponseDto<List<ProductDto>>.Success(200,productsDtos));
            return CreateActionResult(CustomResponseDto<ProductDto>.Success(200, productsDto));
        }

        [HttpPost()]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);

            return CreateActionResult(CustomResponseDto<ProductDto>.Success(201, productsDto));
        }

        [HttpPut()]
        public async Task<IActionResult> Update(ProductUpdateDto productUpdateDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productUpdateDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
        [ServiceFilter(typeof(NotFoundFilter<Product>))]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Success(204));
        }
    }
}
