using Microsoft.AspNetCore.Mvc;
using GSMS.Repositories;
using GSMS.Dtos;
namespace GSMS
{
    [ApiController]
    [Route("stock")]
    public class StocksController : ControllerBase
    {
        private IStockRepository stockRepository;
        
        public StocksController(IStockRepository stockRepository){
            this.stockRepository = stockRepository;
        }

        [HttpGet]
        public ActionResult<Dictionary<string, int>> GetStock(){
            var stock = stockRepository.getQuantities();
            return stock;
        }

        [HttpGet("{name}")]
        public ActionResult<Dictionary<string, int>> GetQuantity(string name){
            bool isItemExists = stockRepository.checkContainsItem(name);

            if(!isItemExists){
                return NotFound();
            }

            var quantity = stockRepository.getQuantityOfAnItem(name);
            return new Dictionary<string, int>(){ [name] = quantity };
        }

        [HttpPost]
        public ActionResult<Dictionary<string, int>> CreateItem(CreateItemDto createItemDto){
            var result = stockRepository.addItem(createItemDto.name, createItemDto.quantity);

            if(!result) {
                return StatusCode(403);
            }
            return new Dictionary<string, int>() {[createItemDto.name] = createItemDto.quantity};
        }

    }
}