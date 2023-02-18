using A2.Models;
using A2.Dtos;
namespace A2.Data
{
    public interface IA2Repo
    {
        IEnumerable<GameRecord> GetAllRecords();
        //GameMove GetMove(string gameid);
        bool AddUser(User user);
        bool ValidLogin(string userName, string password);
        GameRecordOut AvailableGame(string username);
        string getMove(string gameid, string username);
        string MakeMove(string gameid, string username, string move);
        string removeGame(string gameid, string username);
    }
}