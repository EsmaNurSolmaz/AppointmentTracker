using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace RandevuYonetimSistemi.Services
{
    public class AppointmentService
    {
     
        public async Task<List<AppointmentModel>> GetAllAppointmentsAsync()
        {
            using var db = new AppDbContext();

            return await db.Appointments
                .Include(a => a.Customer)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }

       
        public async Task<AppointmentModel?> GetAppointmentByIdAsync(int appointmentId)
        {
            using var db = new AppDbContext();

            return await db.Appointments
                .Include(a => a.Customer)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
        }

       
        public async Task AddAppointmentAsync(AppointmentModel appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            using var db = new AppDbContext();

            if (appointment.CustomerId <= 0)
                throw new Exception("CustomerId cannot be empty.");

            if (appointment.AppointmentDate == default)
                appointment.AppointmentDate = DateTime.Now;

            if (appointment.CreatedAt == default)
                appointment.CreatedAt = DateTime.Now;

            db.Appointments.Add(appointment);
            await db.SaveChangesAsync();
        }

        public async Task UpdateAppointmentAsync(AppointmentModel appointment)
        {
            if (appointment == null)
                throw new ArgumentNullException(nameof(appointment));

            using var db = new AppDbContext();

            var existing = await db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointment.AppointmentId);

            if (existing == null)
                throw new InvalidOperationException("Appointment not found.");

            existing.CustomerId = appointment.CustomerId;
            existing.AppointmentDate = appointment.AppointmentDate;
            existing.Service = appointment.Service;
            existing.Status = appointment.Status;
            existing.Notes = appointment.Notes;

            await db.SaveChangesAsync();
        }


        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            using var db = new AppDbContext();


            var existing = await db.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);


            db.Appointments.Remove(existing);
            await db.SaveChangesAsync();

            MessageBox.Show("✅ Appointment deleted");
        }


        public async Task<List<AppointmentModel>> GetAppointmentsByDateAsync(DateTime date)
        {
            using var db = new AppDbContext();
            var targetDate = date.Date;

            return await db.Appointments
                .Include(a => a.Customer)
                .Where(a => a.AppointmentDate.Date == targetDate)
                .OrderBy(a => a.AppointmentDate)
                .ToListAsync();
        }
    }
}
