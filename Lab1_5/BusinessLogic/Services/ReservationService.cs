using Lab1_5.DataAccess.Repositories;
using Lab1_5.Models.Entity;

namespace Lab1_5.BusinessLogic.Services;

public class ReservationService
{
    private readonly ReservationRepository _reservationRepository;

    public ReservationService(ReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    public void Create(Reservation reservation)
    {
        _reservationRepository.Create(reservation);
    }

    public void Update(Reservation reservation)
    {
        _reservationRepository.Update(reservation);
    }

    public void Delete(Reservation reservation)
    {
        _reservationRepository.Delete(reservation);
    }

    public IEnumerable<Reservation> GetAll()
    {
        return _reservationRepository.GetAll();
    }

    public Reservation? GetById(Guid id)
    {
        return _reservationRepository.GetByCondition(r => r.Id == id);
    }

    public IEnumerable<Reservation> GetByUserEmailWithRooms(string email)
    {
        return _reservationRepository
            .GetAll()
            .Where(r => r.User.Email == email)
            .ToList();
    }
}