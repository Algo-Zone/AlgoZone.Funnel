using AlgoZone.Core.EventData;
using AlgoZone.Funnel.Businesslayer.Models;
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
            CreateMap<Candlestick, SymbolBinanceKline>().ReverseMap();
            CreateMap<Candlestick, SymbolCandlestick>()
                .ForMember(dst => dst.Open, opts => opts.MapFrom(src => src.Open))
                .ForMember(dst => dst.High, opts => opts.MapFrom(src => src.High))
                .ForMember(dst => dst.Low, opts => opts.MapFrom(src => src.Low))
                .ForMember(dst => dst.Close, opts => opts.MapFrom(src => src.Close))
                .ForMember(dst => dst.Volume, opts => opts.MapFrom(src => src.Volume))
                .ForMember(dst => dst.Timestamp, opts => opts.MapFrom(src => src.OpenTime))
                .ForMember(dst => dst.Symbol, opts => opts.MapFrom(src => src.Symbol));
        }

        #endregion
    }
}