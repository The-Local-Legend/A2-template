using Microsoft.EntityFrameworkCore.ChangeTracking;
using A2.Models;
using A2.Dtos;
namespace A2.Data
{
    public class A2Repo: IA2Repo
    {
        private readonly A2DBContext _dbContext;
        public A2Repo(A2DBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<GameRecord> GetAllRecords()
        {
            IEnumerable<GameRecord> records = _dbContext.Gamerecords.ToList<GameRecord>();
            return records;
        }
        public bool AddUser(User user)
        {
            User u = _dbContext.Users.FirstOrDefault(e => e.UserName == user.UserName);
            if(u == null)
            {
                
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return true;
            }
            else
            {
                
                return false;
            }
        }
        public bool ValidLogin(string userName, string password)
        {
            User u = _dbContext.Users.FirstOrDefault(e => e.UserName == userName && e.Password == password);
            if(u == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public GameRecordOut AvailableGame(string username)
        {
            GameRecord g = _dbContext.Gamerecords.FirstOrDefault(e => e.State == "wait");
            if(g == null)
            {
                g = new GameRecord { Player1 = username, State = "wait", LastMovePlayer1 = null, LastMovePlayer2 = null, Player2 = null, GameId = System.Guid.NewGuid().ToString() };
                _dbContext.Gamerecords.Add(g);
                _dbContext.SaveChanges();
               
            }
            else
            {
                if(g.Player1 == username)
                {
                    GameRecordOut go2 = new GameRecordOut { GameId = g.GameId, LastMovePlayer1 = g.LastMovePlayer1, LastMovePlayer2 = g.LastMovePlayer2, Player1 = g.Player1, Player2 = g.Player2, State = g.State };
                    return go2;
                }
                g.Player2 = username;
                g.State = "progress";
                _dbContext.SaveChanges();

            }
            GameRecordOut go = new GameRecordOut { GameId = g.GameId, LastMovePlayer1 = g.LastMovePlayer1, LastMovePlayer2 = g.LastMovePlayer2, Player1 = g.Player1, Player2 = g.Player2, State = g.State };
            return go;
        }
        public string getMove(string gameid, string username)
        {
            GameRecord g = _dbContext.Gamerecords.FirstOrDefault(e => e.GameId == gameid);
            if(g == null)
            {
                return "no such gameId";
            }
            if(!(g.Player1 == username) && !(g.Player2 == username))
            {
                return "not your game id";
            }
            if((g.Player1 == username && g.Player2 == null) || (g.Player1 == null && g.Player2 == username))
            {
                return "You do not have an opponent yet";
            }
            if(g.Player1 == username && g.LastMovePlayer2 == null || g.Player2 == username && g.LastMovePlayer1 == null)
            {
                return "Your opponent has not moved yet";
            }
            if(g.Player1 == username)
            {
                return g.LastMovePlayer2;
            }
            else
            {
                return g.LastMovePlayer1;
            }
        }
        public string MakeMove(string gameid, string username, string move)
        {
            GameRecord g = _dbContext.Gamerecords.FirstOrDefault(e => e.GameId == gameid);
            if (g == null)
            {
                return "no such game id";
            }
            if ((g.Player1 == username && g.Player2 == null) || (g.Player1 == null && g.Player2 == username))
            {
                return "You do not have an opponent yet";
            }
            if(g.Player1 == username)
            {
                if(g.LastMovePlayer1 == null)
                {
                    g.LastMovePlayer1 = move;
                    g.LastMovePlayer2 = null;
                    _dbContext.SaveChanges();
                    return "move registered";
                }
                else
                {
                    return "It is not your turn.";
                }
            }
            if (g.Player2 == username)
            {
                if (g.LastMovePlayer2 == null)
                {
                    g.LastMovePlayer2 = move;
                    g.LastMovePlayer1 = null;
                    _dbContext.SaveChanges();
                    return "move registered";
                }
                else
                {
                    return "It is not your turn.";
                }
            }
            return "no such game id";
        }
        public string removeGame(string gameid, string username)
        {
            GameRecord g = _dbContext.Gamerecords.FirstOrDefault(e => e.GameId == gameid);
            if (g == null)
            {
                return "no such game id";
            }
            if(g.Player1 == username || g.Player2 == username)
            {
                _dbContext.Gamerecords.Remove(g);
                _dbContext.SaveChanges();
                return "game over";
            }
            IEnumerable<GameRecord> games = _dbContext.Gamerecords.Where(x => x.Player1 == username || x.Player2 == username);
            if(games.Count() == 0)
            {
                return "You have not started a game.";
            }
            if(!(g.Player1 == username || g.Player2 == username))
            {
                return "not your game id";
            }
            return "no such game id";
        }
    }
}