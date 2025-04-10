using Lab1_5.DataAccess.Repositories;
using Lab1_5.Models.Entity;

namespace Lab1_5.BusinessLogic.Services;

public class RoomService
{
    private readonly IRepository<Room> _roomRepository;

    public RoomService(IRepository<Room> roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public void Create(Room room)
    {
        throw new NotImplementedException();
    }

    public void Update(Room room)
    {
        throw new NotImplementedException();
    }

    public void Delete(Room room)
    {
        throw new NotImplementedException();
    }

    public Room? GetById(int id)
    {
        return _roomRepository.GetByCondition(r => r.Id == id);
    }

    public IEnumerable<Room> GetAll()
    {
        return _roomRepository.GetAll();
    }



}