using DogsHouse.Core.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DogsHouse.Core.Models.ModelBinders
{
    public class DogDtoModelBinder
    {
        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            try
            {
                var jsonElement = await JsonSerializer.DeserializeAsync<JsonElement>(
                    bindingContext.HttpContext.Request.Body);

                var createDogDto = new DogDto();

                if (jsonElement.TryGetProperty("name", out var nameElement))
                {
                    createDogDto.Name = nameElement.GetString() ?? string.Empty;
                }

                if (jsonElement.TryGetProperty("color", out var colorElement))
                {
                    createDogDto.Color = colorElement.GetString() ?? string.Empty;
                }

                if (jsonElement.TryGetProperty("tailLength", out var tailLengthElement))
                {
                    if (tailLengthElement.ValueKind == JsonValueKind.String)
                    {
                        if (int.TryParse(tailLengthElement.GetString(), out int tailLength))
                        {
                            createDogDto.TailLength = tailLength;
                        }
                        else
                        {
                            bindingContext.ModelState.AddModelError("tailLength",
                                "Tail length must be a valid integer number");
                            return;
                        }
                    }
                    else if (tailLengthElement.ValueKind == JsonValueKind.Number)
                    {
                        createDogDto.TailLength = tailLengthElement.GetInt32();
                    }
                }

                if (jsonElement.TryGetProperty("weight", out var weightElement))
                {
                    if (weightElement.ValueKind == JsonValueKind.String)
                    {
                        if (int.TryParse(weightElement.GetString(), out int weight))
                        {
                            createDogDto.Weight = weight;
                        }
                        else
                        {
                            bindingContext.ModelState.AddModelError("weight",
                                "Weight must be a valid integer number");
                            return;
                        }
                    }
                    else if (weightElement.ValueKind == JsonValueKind.Number)
                    {
                        createDogDto.Weight = weightElement.GetInt32();
                    }
                }

                bindingContext.Result = ModelBindingResult.Success(createDogDto);
            }
            catch (JsonException)
            {
                bindingContext.ModelState.AddModelError(string.Empty,
                    "Invalid JSON format");
            }
        }
    }
}
