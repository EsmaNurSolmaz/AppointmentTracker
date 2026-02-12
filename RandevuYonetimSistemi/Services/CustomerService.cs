using Microsoft.EntityFrameworkCore;
using RandevuYonetimSistemi.Data;
using RandevuYonetimSistemi.Models;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace RandevuYonetimSistemi.Services
{
    public class CustomerService
    {
        public ObservableCollection<CustomerModel> Customers { get; }
            = new ObservableCollection<CustomerModel>();

  
        public async Task LoadCustomersAsync()
        {
            using var db = new AppDbContext();
            Customers.Clear();

            var list = await db.Customers
                .OrderBy(c => c.FullName)
                .ToListAsync();

            foreach (var c in list)
                Customers.Add(c);
        }

        
        public async Task AddCustomerAsync(CustomerModel customer)
        {
            using var db = new AppDbContext();

            if (customer.CreatedAt == default)
                customer.CreatedAt = System.DateTime.Now;

            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            Customers.Add(customer);
        }

   
        public async Task UpdateCustomerAsync(CustomerModel customer)
        {
            using var db = new AppDbContext();
            var existing = await db.Customers.FindAsync(customer.CustomerId);
            if (existing == null) return;

            existing.FullName = customer.FullName;
            existing.PhoneNum = customer.PhoneNum;
            existing.Email = customer.Email;

            db.Customers.Update(existing);
            await db.SaveChangesAsync();

        }

        public async Task DeleteCustomerAsync(int id)
        {
            using var db = new AppDbContext();
            var existing = await db.Customers.FindAsync(id);
            if (existing == null) return;

            db.Customers.Remove(existing);
            await db.SaveChangesAsync();

            var uiItem = Customers.FirstOrDefault(x => x.CustomerId == id);
            if (uiItem != null)
                Customers.Remove(uiItem);
        }
    }
}
