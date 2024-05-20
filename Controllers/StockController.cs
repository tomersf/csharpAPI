using api.Models;
using api.Services;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/stocks")]
    public class StocksController : ControllerBase
    {
        private readonly StockService _stockService;
        public StocksController(StockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<List<Stock>> Get() => await _stockService.GetStocksAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Stock>> Get(string id)
        {
            Stock stock = await _stockService.GetStockAsync(id);
            if (stock == null)
            {
                return NotFound();
            }
            return Ok(stock);
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> Post([FromBody] Stock stock)
        {
            await _stockService.CreateStockAsync(stock);
            return CreatedAtAction(nameof(Get), new { id = stock.Id }, stock);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<ActionResult> Put(string id, [FromBody] Stock updateStock)
        {
            Stock stock = await _stockService.GetStockAsync(id);
            if (stock == null)
            {
                return NotFound("There is no stock with this id");
            }

            updateStock.Id = stock.Id;
            await _stockService.UpdateStockAsync(id, updateStock);

            return Ok("Updated successfully");
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<ActionResult> Delete(string id)
        {
            Stock stock = await _stockService.GetStockAsync(id);
            if (stock == null)
            {
                return NotFound("There is no stock with this id " + id);
            }

            await _stockService.DeleteStockAsync(id);

            return Ok("Deleted successfully");
        }
    }
}