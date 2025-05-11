using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;

namespace Lab1_6.BusinessLogic.Services;

public class RoomService(IUnitOfWork unitOfWork)
{
    public void Update(Room room)
    {
        ArgumentNullException.ThrowIfNull(room);
        unitOfWork.Rooms.Update(room);
        unitOfWork.SaveChanges();
    }

    public Room? GetById(int id)
    {
        return unitOfWork.Rooms.GetByCondition(r => r.Id == id);
    }

    public IEnumerable<Room> GetAll()
    {
        return unitOfWork.Rooms.GetAll();
    }
}