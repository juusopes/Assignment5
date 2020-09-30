using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using MongoDB.Driver;

public class PlayerListHolder
{
    public List<Player> playerList = new List<Player>();
}

public class FileRepository : IRepository
{
    public async Task<Player> Create(Player player)
    {
        PlayerListHolder players = await ReadFile();
        players.playerList.Add(player);
        File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
        return player;
    }

    public async Task<Item> CreateItem(Guid playerId, Item item)
    {
        PlayerListHolder players = await ReadFile();
        Player getPlayerItem = new Player();

        for (int i = 0; i < players.playerList.Count; i++)
        {
            if (players.playerList[i].Id == playerId)
            {
                getPlayerItem = players.playerList[i];
            }
        }

        if (getPlayerItem.itemList == null)
        {
            getPlayerItem.itemList = new List<Item>();
        }

        getPlayerItem.itemList.Add(item);
        File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
        return item;
    }

    public async Task<Item> GetItem(Guid playerId, Guid itemId)
    {
        PlayerListHolder players = await ReadFile();
        Item itemToGet = new Item();

        for (int i = 0; i < players.playerList.Count; i++)
        {
            if (players.playerList[i].Id == playerId)
            {
                for (int j = 0; j < players.playerList[i].itemList.Count; j++)
                {
                    if (players.playerList[i].itemList[j].itemId == itemId)
                    {
                        itemToGet = players.playerList[i].itemList[j];

                        return itemToGet;
                    }
                }
            }
        }

        return null;
    }

    public async Task<Item> DeleteItem(Guid playerId, Guid itemId)
    {
        PlayerListHolder players = await ReadFile();
        Item itemToRemove = new Item();

        for (int i = 0; i < players.playerList.Count; i++)
        {
            if (players.playerList[i].Id == playerId)
            {
                for (int j = 0; j < players.playerList[i].itemList.Count; j++)
                {
                    if (players.playerList[i].itemList[j].itemId == itemId)
                    {
                        itemToRemove = players.playerList[i].itemList[j];
                        players.playerList[i].itemList.RemoveAt(j);
                        File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));

                        return itemToRemove;
                    }
                }
            }
        }

        return null;
    }
    public async Task<Item[]> GetAllItems(Guid playerId)
    {
        PlayerListHolder players = await ReadFile();
        for (int i = 0; i < players.playerList.Count; i++)
        {
            if (players.playerList[i].Id == playerId)
            {
                return players.playerList[i].itemList.ToArray();
            }
        }

        return null;
    }

    public async Task<Item> UpdateItem(Guid playerId, Guid itemId, ModifiedItem item)
    {
        PlayerListHolder players = await ReadFile();

        for (int i = 0; i < players.playerList.Count; i++)
        {
            if (players.playerList[i].Id == playerId)
            {
                for (int j = 0; j < players.playerList[i].itemList.Count; j++)
                {
                    if (players.playerList[i].itemList[j].itemId == itemId)
                    {
                        players.playerList[i].itemList[j].level = item.Level;
                        File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));

                        return players.playerList[i].itemList[j];
                    }
                }
            }
        }

        return null;
    }

    public async Task<Player> Delete(Guid id)
    {
        PlayerListHolder players = await ReadFile();

        Player deletePlayer = new Player();

        for (int i = 0; i < players.playerList.Count; i++)
        {
            if (players.playerList[i].Id == id)
            {
                deletePlayer = players.playerList[i];
                players.playerList.RemoveAt(i);
                File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));

                return deletePlayer;
            }
        }

        return null;
    }

    public async Task<Player> Get(Guid id)
    {
        PlayerListHolder players = await ReadFile();

        Player getPlayer = new Player();

        foreach (Player player in players.playerList)
        {
            if (player.Id == id)
            {
                getPlayer = player;

                return getPlayer;
            }
        }

        return null;
    }

    public async Task<Player[]> GetAll()
    {
        PlayerListHolder players = await ReadFile();
        return players.playerList.ToArray();
    }

    public async Task<Player> Modify(Guid id, ModifiedPlayer player)
    {
        PlayerListHolder players = await ReadFile();
        Player modifyPlayer = new Player();

        foreach (Player oldPlayer in players.playerList)
        {
            if (oldPlayer.Id == id)
            {
                oldPlayer.Score = player.Score;
                modifyPlayer = oldPlayer;
                File.WriteAllText("game-dev.txt", JsonConvert.SerializeObject(players));
            }
        }

        return modifyPlayer;
    }

    public async Task<PlayerListHolder> ReadFile()
    {
        var players = new PlayerListHolder();
        string json = await File.ReadAllTextAsync("game-dev.txt");

        if (File.ReadAllText("game-dev.txt").Length != 0)
        {
            return JsonConvert.DeserializeObject<PlayerListHolder>(json);
        }

        return players;
    }

    public async Task<List<Player>> Ranges(int points)
    {
        PlayerListHolder players = await ReadFile();
        return null;
    }

    public async Task<List<Player>> Sorting()
    {
        PlayerListHolder players = await ReadFile();
        // var filters = Builders<Player>.Filter.Gte(players => players.Level, 19) & Builders<Player>.Filter.Lte(players => players.Level, 30);
        // var players = await ReadFile.Find(filter).ToListAsync();



        return null;
        // return players.ToArray();
    }

    public async Task<Player> SelectorMatching(string playerName)
    {
        PlayerListHolder players = await ReadFile();
        return null;
    }
    

    public async Task<List<Player>> SubDoc(ItemType itemType)
    {
        PlayerListHolder players = await ReadFile();
        return null;
    }

    public async Task<Player> PopAndIncrement(Guid playerId, Guid itemId, int score)
    {
        PlayerListHolder players = await ReadFile();
        return null;
    }
}