using MongoDB.Bson;
using MongoDB.Driver;
using OnAim.Admin.Contracts.Dtos.Coin;
using OnAim.Admin.Contracts.Dtos.Promotion;
using OnAim.Admin.Domain.HubEntities;
using OnAim.Admin.Domain.Interfaces;
using OnAim.Admin.Infrasturcture.Persistance.MongoDB;

namespace OnAim.Admin.Infrasturcture.Repositories
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly AuditLogDbContext _auditLogDbContext;

        public PromotionRepository(AuditLogDbContext auditLogDbContext)
        {
            _auditLogDbContext = auditLogDbContext;
        }

        public async Task<Promotion> UpdatePromotionAsync(ObjectId promotionId, Promotion updatedPromotion)
        {
            var filter = Builders<Promotion>.Filter.Eq(p => p.Id, promotionId);
            await _auditLogDbContext.Promotions.ReplaceOneAsync(filter, updatedPromotion);

            return updatedPromotion;
        }

        public async Task<List<Promotion>> UpdateCoinForPromotionsAsync(List<string> promotionIds, string coinId, CoinDto updatedCoin)
        {
            var promotionObjectIds = promotionIds.Select(ObjectId.Parse).ToList();
            var coinObjectId = ObjectId.Parse(coinId);

            var filter = Builders<Promotion>.Filter.And(
                Builders<Promotion>.Filter.In(p => p.Id, promotionObjectIds),
                Builders<Promotion>.Filter.ElemMatch(p => p.Coins, c => c.Id == coinObjectId)
            );

            var update = Builders<Promotion>.Update
                .Set(p => p.Coins[-1].Name, updatedCoin.Name)
                .Set(p => p.Coins[-1].Url, updatedCoin.Url);

            var updateResult = await _auditLogDbContext.Promotions.UpdateManyAsync(filter, update);

            if (updateResult.MatchedCount == 0)
            {
                throw new InvalidOperationException("No matching promotions or coins found for the given IDs.");
            }

            var updatedPromotions = await _auditLogDbContext.Promotions
                .Find(Builders<Promotion>.Filter.In(p => p.Id, promotionObjectIds))
                .ToListAsync();

            return updatedPromotions;
        }


        public async Task<Promotion> AddAsync(CreatePromotionDto promotion)
        {
            var res = new Promotion
            {
                Name = promotion.Title,
                Description = promotion.Description,
                StartDate = promotion.StartIn,
                EndDate = promotion.EndIn,
                CreateDate = System.DateTime.Now
            };
            var hardcodedCoinIn = new Coin
            {
                Id = ObjectId.Parse("6736ef75d40bbf09bdfc437b"),  
                Name = "CoinIn",
                CoinType = CoinType.CoinIn
            };

            res.AddCoin(hardcodedCoinIn);

            if (promotion.CoinOutDto != null)
            {
                var coinOut = new Coin
                {
                    Name = promotion.CoinOutDto.Name,
                    Url = promotion.CoinOutDto.Url,
                    CoinType = CoinType.CoinOut
                };
                res.AddCoin(coinOut);
            }

            await _auditLogDbContext.Promotions.InsertOneAsync(res);
            return res;
        }

        public async Task<List<Promotion>> GetAllPromotionsAsync(PromotionFilter filter)
        {
            var builder = Builders<Promotion>.Filter;
            var filters = new List<FilterDefinition<Promotion>>();

            if (!string.IsNullOrEmpty(filter.Name))
                filters.Add(builder.Regex(p => p.Name, new BsonRegularExpression(filter.Name, "i")));

            //if (!string.IsNullOrEmpty(filter.Status))
            //    filters.Add(builder.Eq(p => p.PromotionStatus.ToString(), filter.Status));

            if (filter.StartDate.HasValue)
                filters.Add(builder.Gte(p => p.StartDate, filter.StartDate.Value));

            if (filter.EndDate.HasValue)
                filters.Add(builder.Lte(p => p.EndDate, filter.EndDate.Value));

            if (filter.IsActive.HasValue)
                filters.Add(builder.Eq(p => p.IsDeleted, !filter.IsActive.Value));

            var combinedFilter = filters.Count > 0 ? builder.And(filters) : builder.Empty;

            var sortBuilder = Builders<Promotion>.Sort;
            SortDefinition<Promotion> sort = null;

            if (!string.IsNullOrEmpty(filter.SortBy))
            {
                var sortBy = filter.SortBy.ToLower();
                sort = filter.SortDescending.HasValue && filter.SortDescending.Value
                    ? sortBuilder.Descending(sortBy)
                    : sortBuilder.Ascending(sortBy);
            }
            else
            {
                sort = sortBuilder.Descending(p => p.CreateDate);
            }

            var skip = (filter.PageNumber - 1) * (filter.PageSize ?? 10);
            var promotions = await _auditLogDbContext.Promotions
                .Find(combinedFilter)
                .Sort(sort)
                .Skip(skip)
                .Limit(filter.PageSize ?? 10)
                .ToListAsync();

            return promotions;
        }

        public async Task<Promotion> GetPromotionByIdAsync(ObjectId promotionId)
        {
            var filter = Builders<Promotion>.Filter.Eq(p => p.Id, promotionId);

            var promotion = await _auditLogDbContext.Promotions
                .Find(filter)
                .FirstOrDefaultAsync();

            return promotion;
        }
    }
}