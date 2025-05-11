using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;

namespace Lab1_6.BusinessLogic.Services;

public class ReservationService(IUnitOfWork unitOfWork)
{
    public void Create(Reservation reservation)
    {
        unitOfWork.Reservations.Create(reservation);
        unitOfWork.SaveChanges();
    }

    public void Update(Reservation reservation)
    {
        unitOfWork.Reservations.Update(reservation);
        unitOfWork.SaveChanges();
    }

    public void Delete(Reservation reservation)
    {
        unitOfWork.Reservations.Delete(reservation);
        unitOfWork.SaveChanges();
    }

    public IEnumerable<Reservation> GetAll()
    {
        return unitOfWork.Reservations.GetAll();
    }

    public Reservation? GetById(Guid id)
    {
        return unitOfWork.Reservations.GetByCondition(r => r.Id == id);
    }

    public IEnumerable<Reservation> GetByUserEmailWithRooms(string email)
    {
        return unitOfWork.Reservations
            .GetAll()
            .Where(r => r.User.Email == email)
            .ToList();
    }
}