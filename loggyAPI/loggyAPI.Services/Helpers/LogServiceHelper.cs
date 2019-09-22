using System;
using loggyAPI.Data.Entities;

namespace loggyAPI.Services.Helpers
{
    public static class LogServiceHelper
    {
        public static void ValidateProject(this Project project)
        {
            if (!string.IsNullOrEmpty(project.Name))
            {
                throw new ArgumentException("Project name can't be empty.", nameof(project.Name));
            }

            if (project.User == null)
            {
                throw new ArgumentException("User can't be null.", nameof(project.User));
            }
        }
    }
}
