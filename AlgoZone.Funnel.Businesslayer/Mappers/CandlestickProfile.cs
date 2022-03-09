using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Datalayer.Binance;
using AutoMapper;

namespace AlgoZone.Funnel.Businesslayer.Mappers
{
    public class CandlestickProfile : Profile
    {
        #region Constructors

        public CandlestickProfile()
        {
            CreateMap<BinanceSymbolEvent<SymbolBinanceKline>, SymbolCandlestickEventData>()
                .AfterMap((src, dst) => dst.Data.Symbol = src.Topic);
            CreateMap<SymbolBinanceKline, SymbolCandlestick>();
        }

        #endregion
    }
}