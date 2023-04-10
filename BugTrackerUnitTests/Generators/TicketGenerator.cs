using BugTrackerByBenci.Models;

namespace BugTrackerUnitTests.Generators
{
    public static class TicketGenerator
    {
        public static List<Ticket> GenerateTicket()
        {
            List<Ticket> tickets = new();

            for (int i = 0; i <= 10; i++)
            {
                Ticket ticket = new()
                {
                    Id = i + 1,
                    Title = "Test",
                    Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    Description = "Testing repository",
                    SubmitterUserId = Guid.NewGuid().ToString(),
                    Comments = new HashSet<TicketComment> { new TicketComment() { Comment = "Test Comment", UserId = Guid.NewGuid().ToString() } },
                    Attachments = new HashSet<TicketAttachment> { new TicketAttachment() { UserId = Guid.NewGuid().ToString() } },
                    History = new HashSet<TicketHistory> { new TicketHistory() { UserId = Guid.NewGuid().ToString() } },
                    DeveloperUser = new BTUser() { FirstName = "Test First Name Developer", LastName = "Test Last Name of Developer" },
                    SubmitterUser = new BTUser() { FirstName = "Test First Name Submitter", LastName = "Test Last Name of Submitter" },
                    TicketStatus = new TicketStatus() { Name = "Test Status" },
                    TicketPriority = new TicketPriority() { Name = "Test Ticket Priority" },
                    TicketType = new TicketType() { Name = "Test Ticket Type" },
                    ProjectId = i+ 1,
                    Notifications = new HashSet<Notification> { new Notification()
                        {
                            Title = "Test Notification",
                            Message = "Test Message",
                            Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                            SenderId = Guid.NewGuid().ToString(),
                            RecipientId = Guid.NewGuid().ToString()
                        }
                    }
                };
                tickets.Add(ticket);
            }
            return tickets;
        }
    }
}
