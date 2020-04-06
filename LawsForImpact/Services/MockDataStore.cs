using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LawsForImpact.Models;

namespace LawsForImpact.Services
{
    public class MockDataStore : IDataStore<Item>
    {
        readonly List<Item> items;

        public MockDataStore()
        {
            items = new List<Item>()
            {
                new Item { Id = Guid.NewGuid().ToString(), Text = "Personal", Description="User input info" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Power", Description="The 48 Laws of Power" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "War", Description="The 33 Laws of War" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Mastery", Description="The Principles of Mastery" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Friends", Description="How to Win Friends and Influence Others" },
                new Item { Id = Guid.NewGuid().ToString(), Text = "Human", Description="Laws of Human Dynamics." }
            };
        }

        public async Task<bool> AddItemAsync(Item item)
        {
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Item item)
        {
            var oldItem = items.Where((Item arg) => arg.Id == item.Id).FirstOrDefault();
            items.Remove(oldItem);
            items.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = items.Where((Item arg) => arg.Id == id).FirstOrDefault();
            items.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Item> GetItemAsync(string id)
        {
            return await Task.FromResult(items.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Item>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(items);
        }
    }
}