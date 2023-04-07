using BugTrackerByBenci.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BugTrackerUnitTests.Generators
{
    public static class ProjectGenerator
    {
        public static List<Project> GenerateProject(int count)
        {
            List<Project> projects = new List<Project>();

            for (int i = 0; i < count; i++)
            {
                var project = new Project()
                {
                    CompanyId = 1,
                    Name = "Build a Personal Porfolio",
                    Description = "Single page html, css & javascript page.  Serves as a landing page for candidates and contains a bio and links to all applications and challenges.",
                    Created = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc),
                    StartDate = DateTime.SpecifyKind(new DateTime(2021, 8, 20), DateTimeKind.Utc),
                    EndDate = DateTime.SpecifyKind(new DateTime(2021, 8, 20).AddMonths(1), DateTimeKind.Utc),
                    ProjectPriorityId = 0
                };

                projects.Add(project);
            }

            return projects;
        }
    }
}
