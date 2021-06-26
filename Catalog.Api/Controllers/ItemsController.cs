using Microsoft.AspNetCore.Mvc;
using Catalog.Repositories;
using System.Collections.Generic;
using Catalog.Entities;
using System;
using System.Linq;
using Catalog.Dtos;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Catalog.Contollers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository repository;
        private readonly ILogger<ItemsController> logger;

        public ItemsController(IItemsRepository _repository, ILogger<ItemsController> _logger)
        {
            this.repository = _repository;
            this.logger = _logger;
        }

        [HttpGet]
        public async Task<IEnumerable<ItemDto>> GetItemsAsync()
        {
            var items = (await repository.GetItemsAsync())
                        .Select(item => item.AsDto());

            logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm::ss")}: Retrieved - {items.Count()}");
            return items;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
        {
            var item = await repository.GetItemAsync(id);
            if (item is null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
        {
            {
                Item item = new()
                {
                    Id = Guid.NewGuid(),
                    Name = itemDto.Name,
                    Price = itemDto.Price,
                    CreatedDate = DateTimeOffset.UtcNow
                };

                await repository.CreateItemAsync(item);

                return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var existingItem = repository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            Item updatedItem = await existingItem with  // with expression making copy of obj and initailizing
            {
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var existingItem = repository.GetItemAsync(id);
            if (existingItem is null)
            {
                return NotFound();
            }
            await repository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}