using Lab1_5.DataAccess;
using Lab1_5.DataAccess.Interfaces;
using Lab1_5.Models.Entity;

namespace Lab1_5.BusinessLogic.Services;

public class RoomService
{
    private readonly IUnitOfWork _unitOfWork;

    public RoomService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void Create(Room room)
    {
        throw new NotImplementedException();
    }

    public void Update(Room room)
    {
        ArgumentNullException.ThrowIfNull(room);
        _unitOfWork.Rooms.Update(room);
        _unitOfWork.SaveChanges();
    }

    public void Delete(Room room)
    {
        throw new NotImplementedException();
    }

    public Room? GetById(int id)
    {
        return _unitOfWork.Rooms.GetByCondition(r => r.Id == id);
    }

    public IEnumerable<Room> GetAll()
    {
        return _unitOfWork.Rooms.GetAll();
    }
}