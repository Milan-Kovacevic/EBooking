﻿using EBooking.Domain.DAOs;
using EBooking.EntityFramework.DbContext;
using EBooking.EntityFramework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBooking.EntityFramework.DataAccess
{
    public class SQLiteDAOFactory : IGenericDAOFactory
    {
        /// <summary>
        /// Class extenders have ability to change validation of connection string (ex. Based on type of DBMS - SQLite or MySQL),<br/>
        /// but to keep the same logic for creation of dao classes.<br/>
        /// EF offers same abstraction on data access for different database types by using appropriate EFCore packages
        /// </summary>
        /// <param name="connectionString"></param>
        /// <exception cref="DataAccessException"></exception>
        internal void ValidateConnectionString(string connectionString)
        {
            DbConnectionStringBuilder builder = new DbConnectionStringBuilder();
            builder.ConnectionString = connectionString;
            if (!builder.ContainsKey("Data Source") && !builder.ContainsKey("DataSource"))
                throw new DataAccessException($"Invalid connection {connectionString}");
        }

        internal virtual void DoMigration(EBookingDbContext dbContext)
        {
            try
            {
                dbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Unable to create database migration.", ex);
            }
        }

        public SQLiteDAOFactory(string connectionString, bool migrate = true)
        {
            ValidateConnectionString(connectionString);

            EBookingDbContextFactory contextFactory = new EBookingDbContextFactory() { ConnectionString = connectionString };

            if (migrate)
            {
                using var dbContext = contextFactory.Create();
                DoMigration(dbContext);
            }

            // Initialization of DAOs implementations...
        }
    }
}