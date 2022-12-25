using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace PeliculaBackEnd.Utilidades
{
    public class TypeBinder<T> : IModelBinder

    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {

            var nombrePropiedad = bindingContext.ModelName;
            var valor = bindingContext.ValueProvider.GetValue(nombrePropiedad);
            if(valor == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }
            try
            {
                var valorDeserialziado = JsonConvert.DeserializeObject<T>(valor.FirstValue);
                bindingContext.Result = ModelBindingResult.Success(valorDeserialziado);
            }
            catch
            {
                bindingContext.ModelState.TryAddModelError(nombrePropiedad, " El valor dado no es de tipo adecuado");
            }

            return Task.CompletedTask;
        }
    }
}
