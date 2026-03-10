using Mapster;

namespace PosNet.UseCases.Dtos
{
    public abstract class BaseDto<TDto, TModel> : IRegister
        where TDto : class, new()
        where TModel : class, new()
    {

        // Accept config as a parameter
        public virtual void AddCustomMappings(TypeAdapterConfig config) { }

        protected TypeAdapterSetter<TDto, TModel> SetCustomMappings(TypeAdapterConfig config)
            => config.ForType<TDto, TModel>();

        protected TypeAdapterSetter<TModel, TDto> SetCustomMappingsReverse(TypeAdapterConfig config)
            => config.ForType<TModel, TDto>();

        public void Register(TypeAdapterConfig config)
        {
            AddCustomMappings(config);
        }

        public TModel ToModel() => this.Adapt<TModel>();

        public static TDto FromModel(TModel model) => model.Adapt<TDto>();
    }
}
