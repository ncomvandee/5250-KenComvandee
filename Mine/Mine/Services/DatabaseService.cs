﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mine.Models;

namespace Mine.Services
{
    public class DatabaseService : IDataStore<ItemModel>
    {
        static readonly Lazy<SQLiteAsyncConnection> lazyInitializer = new Lazy<SQLiteAsyncConnection>(() =>
        {
            return new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        });

        static SQLiteAsyncConnection Database => lazyInitializer.Value;
        static bool initialized = false;

        public DatabaseService()
        {
            InitializeAsync().SafeFireAndForget(false);
        }

        async Task InitializeAsync()
        {
            if (!initialized)
            {
                if (!Database.TableMappings.Any(m => m.MappedType.Name == typeof(ItemModel).Name))
                {
                    await Database.CreateTablesAsync(CreateFlags.None, typeof(ItemModel)).ConfigureAwait(false);
                }
                initialized = true;
            }
        }

        /// <summary>
        /// Create item to Database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True if successfully added, else return false</returns>
        public async Task<bool> CreateAsync(ItemModel item)
        {
            
            if (item == null)
            {
                return false;
            }

            // Id of what was written in Database Table
            var result = await Database.InsertAsync(item);
            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Updates the item information in Database
        /// </summary>
        /// <param name="item"></param>
        /// <returns>True id succcessfully updated, else if not</returns>
        public async Task<bool> UpdateAsync(ItemModel item)
        {
            if (item == null)
            {
                return false;
            }

            // Id of what was written in Database Table
            var result = await Database.UpdateAsync(item);

            if (result == 0)
            {
                return false;
            }

            return true;
            
        }

        /// <summary>
        /// Delete item from Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>True if successfully deleted, else return false</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            // Find Item by ID
            var data = await ReadAsync(id);

            if (data == null)
            {
                return false;
            }

            // ID of item to be deleted in Database
            var result = await Database.DeleteAsync(data);

            if (result == 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Read the item information from Database
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ItemModel</returns>
        public Task<ItemModel> ReadAsync(string id)
        {
            if (id == null)
            {
                return null;
            }

            // Call the Database to read the id
            // Using Linq syntax, find the first record that match the id
            var result = Database.Table<ItemModel>().FirstOrDefaultAsync(mbox => mbox.Id.Equals(id));

            return result;
        }


        /// <summary>
        /// Initialize index page
        /// </summary>
        /// <param name="forceRefresh"></param>
        /// <returns>result</returns>
        public async Task<IEnumerable<ItemModel>> IndexAsync(bool forceRefresh = false)
        {
            // Call to the ToListAsync method on Database with the Table called ItemModel
            var result = await Database.Table<ItemModel>().ToListAsync();

            return result;
        }

        //...
    }
}
