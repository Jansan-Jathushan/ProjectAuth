using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Projectauth.Models;

namespace Projectauth.Services
{
    public class ProductService
    {
        private readonly IMongoCollection<Product> _products;

        public ProductService(IOptions<MongoDBSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            var database = client.GetDatabase(settings.Value.DatabaseName);
            _products = database.GetCollection<Product>("Products");
        }

        public async Task<List<Product>> GetAll() => await _products.Find(_ => true).ToListAsync();

        public async Task<Product?> GetById(string id) =>
            await _products.Find(p => p.Id == id).FirstOrDefaultAsync();

        public async Task Create(Product product) => await _products.InsertOneAsync(product);

        public async Task Update(string id, Product product) =>
            await _products.ReplaceOneAsync(p => p.Id == id, product);

        public async Task Delete(string id) => await _products.DeleteOneAsync(p => p.Id == id);
    }
}
