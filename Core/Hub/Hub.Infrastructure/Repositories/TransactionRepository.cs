﻿using Hub.Domain.Abstractions.Repository;
using Hub.Domain.Entities;
using Hub.Infrastructure.DataAccess;

namespace Hub.Infrastructure.Repositories;

public class TransactionRepository(HubDbContext context) : BaseRepository<HubDbContext, Transaction>(context), ITransactionRepository
{
}