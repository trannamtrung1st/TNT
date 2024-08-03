﻿using TNT.Layers.Application.Models.Abstracts;
using TNT.Layers.Domain;
using TNT.Layers.Domain.Exceptions;
using TNT.Layers.Domain.Models;

namespace TNT.Layers.Application.Models
{
    public abstract class BaseFilterQuery : IPagingQuery, ISearchQuery
    {
        public BaseFilterQuery() { }
        public BaseFilterQuery(string terms, int skip, int? take)
        {
            Terms = terms;
            Skip = skip;
            Take = take;

            if (!IsPageSizeValid())
                throw new BadRequestException(
                    new ValueDetails(valueName: nameof(Take), detailCode: DetailCodes.OutOfRange));
        }

        protected virtual int TakeMax => QueryDefaults.TakeMax;
        protected virtual bool IsPageSizeValid() => Take > 0 && Take <= TakeMax;
        public string Terms { get; }
        public int Skip { get; }
        public int? Take { get; }

        public abstract bool CanGetAll();
    }
}
