using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class MockDataStore : IDataStore<ItemModel>
    {
        readonly List<ItemModel> items;

        public MockDataStore()
        {
            items = new List<ItemModel>()
            {
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Anywhere Door", Description="Allows you to travel to anywhere in the world", Value = 6},
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Bamboo Helicopter", Description="Attach on your head, fly around", Value = 5 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Translation Jelly", Description="You now become expert in any languaged", Value = 7 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Memorable Bread", Description="You won't forget anything, anymore", Value = 8 },
                new ItemModel { Id = Guid.NewGuid().ToString(), Text = "Almighty Pass", Description="Wanna go to the Oval Office ? No problem", Value = 10 }
            };
        }

        public async Task<bool> CreateAsync(ItemModel item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateAsync(ItemModel item)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((ItemModel arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<ItemModel> ReadAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<ItemModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}