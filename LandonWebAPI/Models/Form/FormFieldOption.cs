﻿using Newtonsoft.Json;

namespace LandonWebAPI.Models.Form;

public class FormFieldOption
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string Label { get; set; }

    public object Value { get; set; }
}
