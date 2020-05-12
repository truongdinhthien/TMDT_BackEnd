using System.Threading.Tasks;
using CartApi.Models;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using Newtonsoft.Json;
using System.Linq;
using System.Collections.Generic;
using System;

namespace CartApi.Services
{
    public class CartService
    {
        private IDatabase database;
        public CartService(IDatabase database)
        {
            this.database = database;
        }
        public async Task<Cart> GetCartAsync (string key)
        {
            var dataSource = await database.StringGetAsync(key);

            var data = dataSource.IsNullOrEmpty ? null : JsonConvert.DeserializeObject<Cart>(dataSource);

            return data ?? new Cart (key);
        }

        public async Task<Cart> UpdateCartAsync (Cart cart)
        {
            var data = JsonConvert.SerializeObject(cart);

            var created = await database.StringSetAsync(cart.buyerId, data);

            return !created ? null : await GetCartAsync(cart.buyerId);
        }

        public async Task AddItemToCart(string id, CartItem cartItem)
        {
            var cartSource = await GetCartAsync(id);

            var itemFound = cartSource.Items.Find(x => x.Id == cartItem.Id);

            if (itemFound == null)
            {
                cartSource.Items.Add(cartItem);
            }
            else
            {
                itemFound.Amount += cartItem.Amount;
            }

            await UpdateCartAsync(cartSource);
        }

        public async Task PutItemToCart(string id, CartItem cartItem)
        {
            var cartSource = await GetCartAsync(id);

            var itemFound = cartSource.Items.Find(x => x.Id == cartItem.Id);

            if (itemFound == null)
            {
                cartSource.Items.Add(cartItem);
            }
            else
            {
                itemFound.Amount = cartItem.Amount;
            }

            await UpdateCartAsync(cartSource);
        }

        public async Task<bool> DeleteCartAsync (string id)
        {
            return await database.KeyDeleteAsync(id);
        }

        public async Task DeleteCartItemAsync(string buyerId, int id)
        {
            var cartSource = await GetCartAsync(buyerId);

            var itemFound = cartSource.Items.Find(x => x.Id == id);

            if (itemFound != null)
            {
                cartSource.Items.Remove(itemFound);
            } else return;

            await UpdateCartAsync(cartSource);
        }

    }
}