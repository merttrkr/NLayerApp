using AutoMapper;
using Microsoft.AspNetCore.Http;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.Repositiories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayer.Service.Services
{
    public class ProductService : Service<Product, ProductDto>, IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, IMapper mapper, IProductRepository productRepository) : base(repository, unitOfWork, mapper)
        {
            _productRepository = productRepository;

        }

        public async Task<CustomResponseDto<ProductDto>> AddAsync(ProductCreateDto dto)
        {
            var newEntity = _mapper.Map<Product>(dto);
            await _productRepository.AddAsync(newEntity);
            await _unitOfWork.CommitAsync();

            var newDto =_mapper.Map<ProductDto>(dto);

            return CustomResponseDto<ProductDto>.Success(StatusCodes.Status201Created, newDto);
        }

        public async Task<CustomResponseDto<IEnumerable<ProductWithCategoryDto>>> GetProductsWithCategory()
        {
            var entities = await _productRepository.GetProductsWithCategory();
            var dtos = _mapper.Map<IEnumerable<ProductWithCategoryDto>>(entities);

            return CustomResponseDto<IEnumerable<ProductWithCategoryDto>>.Success(StatusCodes.Status200OK, dtos);
        }

        public async Task<CustomResponseDto<NoContentDto>> UpdateAsync(ProductUpdateDto dto)
        {
            var entity = _mapper.Map<Product>(dto);
             _productRepository.Update(entity);
            await _unitOfWork.CommitAsync();

            return CustomResponseDto<NoContentDto>.Success(StatusCodes.Status202Accepted);
        }
    }

    
}
