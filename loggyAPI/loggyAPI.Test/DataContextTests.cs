using System;
using System.Collections.Generic;
using loggyAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using NUnit.Framework;

namespace loggyAPI.Test
{
    public class DataContextTests
    {
        [Test]
        public void DataContext()
        {
            var dataContext = new DataContext(new DbContextOptions<DataContext>());
        }
    }
}
