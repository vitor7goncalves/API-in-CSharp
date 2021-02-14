using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testeef.Data;
using testeef.Models;

namespace testeef.Controllers{
    [ApiController]
    [Route("v1/products")]
    public class ProductController: ControllerBase{
           [HttpGet]
           [Route("")]
           public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context){
               var products = await context.Products.Include(x => x.Category).ToListAsync();
               return products;
           }
           [HttpGet]
           [Route("{id:int}")]
           public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id){
               var product = await context.Products.Include(x => x.Category)
                    .AsNoTracking() // --> Nao criar proxy e se voce so quer jogar os dados na tela
                    .FirstOrDefaultAsync(x => x.Id == id);
               return product;
           }
           [HttpGet]
           [Route("categories/{id:int}")]
           public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id){
               var products = await context.Products.Include(x => x.Category)
                    .AsNoTracking() // --> Nao criar proxy e se voce so quer jogar os dados na tela
                    .Where(x => x.CategoryId == id)
                    .ToListAsync(); // Por sempre o tolist é sempre no fim, se or antes de where ele vai pegar todos os dados do banco e vai filtrar isso na memoria
               return products;
           }
           [HttpPost]
           [Route("")]
           public async Task<ActionResult<Product>> Post(
               [FromServices] DataContext context,
               [FromBody]Product model){
               if(ModelState.IsValid){
                   context.Products.Add(model);
                   await context.SaveChangesAsync();
                   return model;
               }else{
                   return BadRequest(ModelState);
               }
           }
    }

}

    
