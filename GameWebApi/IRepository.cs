using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface IRepository
{
    Task<Player> Get(Guid id);
    Task<Player[]> GetAll();
    Task<Player> Create(Player player);
    Task<Player> Modify(Guid id, ModifiedPlayer player);
    Task<Player> Delete(Guid id);

    Task<Item> CreateItem(Guid playerId, Item item);
    Task<Item> GetItem(Guid playerId, Guid itemId);
    Task<Item[]> GetAllItems(Guid playerId);
    Task<Item> UpdateItem(Guid playerId, Guid itemId, ModifiedItem item);
    Task<Item> DeleteItem(Guid playerId, Guid itemId);
    Task<List<Player>> Ranges(int points);
    Task<Player> SelectorMatching(string playerName);
    Task<List<Player>> SubDoc(ItemType itemType);
    Task<List<Player>> Sorting();
    Task<Player> PopAndIncrement(Guid playerId, Guid itemId, int score);
}