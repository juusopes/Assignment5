using System;
using System.Threading.Tasks;
using MongoDB.Bson;
using System.Collections.Generic;
using MongoDB.Driver;

public class MongoDbRepository : IRepository
{
    private readonly IMongoCollection<Player> _playerCollection;
    private readonly IMongoCollection<BsonDocument> _bsonDocumentCollection;
    public MongoDbRepository()
    {
        MongoClient mongoClient = new MongoClient("mongodb://localhost:27017");
        var database = mongoClient.GetDatabase("game");
        _playerCollection = database.GetCollection<Player>("players");

        _bsonDocumentCollection = database.GetCollection<BsonDocument>("players");
    }
    public async Task<Player> Create(Player player)
    {
        await _playerCollection.InsertOneAsync(player);

        return player;
    }

    public async Task<Item> CreateItem(Guid playerId, Item item)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
        Player player = await _playerCollection.Find(filter).FirstAsync();

        if (player == null)
        {
            throw new NotFoundException("Player not found.");
        }

        if (player.itemList == null)
            player.itemList = new List<Item>();
        player.itemList.Add(item);
        await _playerCollection.ReplaceOneAsync(filter, player);

        return item;
    }

    public async Task<Player> Delete(Guid id)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, id);

        return await _playerCollection.FindOneAndDeleteAsync(filter);
    }

    public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
        Player player = await _playerCollection.Find(filter).FirstAsync();
        Item itemToRemove = null;

        for (int j = 0; j < player.itemList.Count; j++)
        {
            if (player.itemList[j].itemId == itemId)
            {
                itemToRemove = player.itemList[j];
                player.itemList.RemoveAt(j);
                await _playerCollection.ReplaceOneAsync(filter, player);

                return itemToRemove;
            }
        }

        return itemToRemove;
    }

    public async Task<Player> Get(Guid id)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, id);

        return await _playerCollection.Find(filter).FirstAsync();
    }

    public async Task<Player[]> GetAll()
    {
        var filter = Builders<Player>.Filter.Empty;
        List<Player> players = await _playerCollection.Find(filter).ToListAsync();

        return players.ToArray();
    }

    public async Task<Item[]> GetAllItems(Guid playerId)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
        Player player = await _playerCollection.Find(filter).FirstAsync();

        return player.itemList.ToArray();
    }

    public async Task<Item> GetItem(Guid playerId, Guid itemId)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
        Player player = await _playerCollection.Find(filter).FirstAsync();

        for (int i = 0; i < player.itemList.Count; i++)
        {
            if (player.itemList[i].itemId == itemId)
                return player.itemList[i];
        }

        return null;
    }

    public async Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, id);
        Player player2 = await _playerCollection.Find(filter).FirstAsync();
        player2.Score = player.Score;
        await _playerCollection.ReplaceOneAsync(filter, player2);

        return player2;
    }

    public async Task<Item> UpdateItem(Guid playerId, Guid itemId, ModifiedItem item)
    {
        var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);
        Player player = await _playerCollection.Find(filter).FirstAsync();

        for (int i = 0; i < player.itemList.Count; i++)
        {
            if (player.itemList[i].itemId == itemId)
            {
                player.itemList[i].level = item.Level;
                await _playerCollection.ReplaceOneAsync(filter, player);

                return player.itemList[i];
            }
        }

        return null;
    }

    public async Task<List<Player>> Ranges(int points)
    {
        var filter = Builders<Player>.Filter.Gt(player => player.Score, points);
        List<Player> players = await _playerCollection.Find(filter).ToListAsync();

        return players;
    }

    public async Task<Player> SelectorMatching(string playerName)
    {
        var filter = Builders<Player>.Filter.Eq(player => player.Name, playerName);
        List<Player> players = await _playerCollection.Find(filter).ToListAsync();
        Player player = players[0];

        return player;
    }

    public async Task<List<Player>> SubDoc(ItemType itemType)
    {
        var playersWithWeapons = Builders<Player>.Filter.ElemMatch<Item>(p => p.itemList, Builders<Item>.Filter.Eq(item => item.type, itemType));

        return await _playerCollection.Find(playersWithWeapons).ToListAsync();
    }

    public async Task<List<Player>> Sorting()
    {
        SortDefinition<Player> sortDef = Builders<Player>.Sort.Ascending("Score");
        IFindFluent<Player, Player> cursor = _playerCollection.Find("").Sort(sortDef).Limit(10);
        List<Player> players = await cursor.ToListAsync();

        return players;
    }

    public Task<Player> PopAndIncrement(Guid playerId, Guid itemId, int score)
    {
        var pull = Builders<Player>.Update.PullFilter(p => p.itemList, i => i.itemId == itemId);
        var increment = Builders<Player>.Update.Inc(p => p.Score, score);
        var update = Builders<Player>.Update.Combine(pull, increment);
        var filter = Builders<Player>.Filter.Eq(p => p.Id, playerId);

        return _playerCollection.FindOneAndUpdateAsync(filter, update);
    }
}