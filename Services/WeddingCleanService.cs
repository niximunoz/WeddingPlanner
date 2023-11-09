using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WeddingPlanner.Models;

public class WeddingCleanService
{
    private readonly IServiceProvider _serviceProvider;

    public WeddingCleanService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task RemoveExpiredWeddingsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyContext>();

        var currentDate = DateTime.Now;
        var expiredWeddings = dbContext.Weddings
            .Where(w => w.Date < currentDate)
            .ToList();

        foreach (var wedding in expiredWeddings)
        {
            var attendees = dbContext.Assistances
                .Where(a => a.WeddingId == wedding.WeddingId)
                .ToList();

            foreach (var attendee in attendees)
            {
                dbContext.Assistances.Remove(attendee);
            }

            dbContext.Weddings.Remove(wedding);
        }

        await dbContext.SaveChangesAsync(); 
    }

}
