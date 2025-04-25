using Lab1_6.DataAccess.Interfaces;
using Lab1_6.Models.Entity;

namespace Lab1_6.BusinessLogic.Services;

public class ReservationService
{
    private readonly IUnitOfWork _unitOfWork;

    public ReservationService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public void Create(Reservation reservation)
    {
        _unitOfWork.Reservations.Create(reservation);
        _unitOfWork.SaveChanges();
    }

    public void Update(Reservation reservation)
    {
        _unitOfWork.Reservations.Update(reservation);
        _unitOfWork.SaveChanges();
    }

    public void Delete(Reservation reservation)
    {
        _unitOfWork.Reservations.Delete(reservation);
        _unitOfWork.SaveChanges();
    }

    public IEnumerable<Reservation> GetAll()
    {
        return _unitOfWork.Reservations.GetAll();
    }

    public Reservation? GetById(Guid id)
    {
        return _unitOfWork.Reservations.GetByCondition(r => r.Id == id);
    }

    public IEnumerable<Reservation> GetByUserEmailWithRooms(string email)
    {
        return _unitOfWork.Reservations
            .GetAll()
            .Where(r => r.User.Email == email)
            .ToList();
    }
}